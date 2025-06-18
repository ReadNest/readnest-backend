using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.ChatMessage;
using ReadNest.Domain.Entities;

namespace ReadNest.BackgroundServices
{
    public class ChatMessageSaver : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public ChatMessageSaver(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(3), stoppingToken); // Đợi 3 phút

                    using var scope = _scopeFactory.CreateScope();
                    var redisQueue = scope.ServiceProvider.GetRequiredService<IRedisChatQueue>();
                    var chatMessageUseCase = scope.ServiceProvider.GetRequiredService<IChatMessageUseCase>();

                    var messages = new List<ChatMessage>();

                    while (true)
                    {
                        var rawMessage = await redisQueue.DequeueRawMessageAsync();
                        if (rawMessage == null) break;

                        var chatMessage = JsonConvert.DeserializeObject<ChatMessage>(rawMessage);
                        if (chatMessage != null)
                        {
                            messages.Add(chatMessage);
                        }
                    }

                    if (messages.Count > 0)
                    {
                        //using var scope = _scopeFactory.CreateScope();
                        //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                        //dbContext.ChatMessages.AddRange(messages);
                        //await dbContext.SaveChangesAsync(stoppingToken);
                        await chatMessageUseCase.SaveRangeMessageAsync(messages); // Chờ hoàn thành lưu trữ

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
