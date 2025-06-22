using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.ChatMessage;
using ReadNest.Domain.Entities;

namespace ReadNest.BackgroundServices
{
    public class ChatMessageSaver : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ChatMessageSaver> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(3); // Thời gian chờ giữa các lần lưu trữ

        public ChatMessageSaver(IServiceScopeFactory scopeFactory, ILogger<ChatMessageSaver> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(_interval, stoppingToken); // Đợi 3 phút

                    using var scope = _scopeFactory.CreateScope();
                    var redisQueue = scope.ServiceProvider.GetRequiredService<IRedisChatQueue>();
                    var chatMessageUseCase = scope.ServiceProvider.GetRequiredService<IChatMessageUseCase>();

                    var messages = new List<ChatMessage>();

                    while (true)
                    {
                        var rawPendingMessage = await redisQueue.DequeuePendingMessageAsync();
                        if (string.IsNullOrEmpty(rawPendingMessage)) break;

                        var chatPendingMessage = JsonConvert.DeserializeObject<ChatMessage>(rawPendingMessage);
                        if (chatPendingMessage != null)
                        {
                            messages.Add(chatPendingMessage);
                        }
                    }

                    if (messages.Count > 0)
                    {
                        //using var scope = _scopeFactory.CreateScope();
                        //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        //dbContext.ChatMessages.AddRange(messages);
                        //await dbContext.SaveChangesAsync(stoppingToken);
                        await chatMessageUseCase.SaveRangeMessageAsync(messages); // Chờ hoàn thành lưu trữ

                        //Sau khi flush → Refresh lại Redis cache từ DB
                        var distinctPairs = messages
                        .Select(m => new { m.SenderId, m.ReceiverId })
                        .Distinct();
                        foreach (var pair in distinctPairs)
                        {
                            var userAId = pair.SenderId;
                            var userBId = pair.ReceiverId;
                            // Lấy toàn bộ cuộc trò chuyện từ DB
                            var fullConversation = await chatMessageUseCase.GetFullConversationAsync(pair.SenderId, pair.ReceiverId);
                            // Xóa cache cũ và lưu mới trong Redis
                            await redisQueue.RefreshConversationCacheAsync(pair.SenderId, pair.ReceiverId, fullConversation.Data);
                        }
                        _logger.LogInformation($"Saved {messages.Count} messages and refreshed Redis cache for {distinctPairs.Count()} conversations.");

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error during batch save: {ex.Message}");
                }
            }
        }
    }
}
