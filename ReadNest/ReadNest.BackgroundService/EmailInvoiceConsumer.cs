using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReadNest.Application.Services;
using ReadNest.Domain.Events;
using StackExchange.Redis;

namespace ReadNest.BackgroundServices
{
    public class EmailInvoiceConsumer : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IServiceProvider _services;
        private const string StreamKey = "email:invoice:stream";
        private const string GroupName = "email-invoice-group";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="redis"></param>
        /// <param name="services"></param>
        public EmailInvoiceConsumer(IConnectionMultiplexer redis, IServiceProvider services)
        {
            _redis = redis;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();
            try
            {
                await db.StreamCreateConsumerGroupAsync(StreamKey, GroupName, "0-0", true);
            }
            catch (RedisServerException ex) when (ex.Message.Contains("BUSYGROUP"))
            {
                Console.WriteLine(ex.Message);
            }

            var consumerName = Environment.MachineName;

            while (!stoppingToken.IsCancellationRequested)
            {
                var entries = await db.StreamReadGroupAsync(StreamKey, GroupName, consumerName, count: 10, noAck: false);

                if (entries.Length == 0)
                {
                    await Task.Delay(1000, stoppingToken);
                    continue;
                }

                foreach (var entry in entries)
                {
                    try
                    {
                        var json = entry.Values.FirstOrDefault(v => v.Name == "data").Value;
                        var emailEvent = JsonSerializer.Deserialize<InvoiceEmailEvent>(json);

                        using var scope = _services.CreateScope();
                        var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                        await emailService.SendEmailAsync(emailEvent.Email, emailEvent.Subject, emailEvent.Body);

                        await db.StreamAcknowledgeAsync(StreamKey, GroupName, entry.Id);
                    }
                    catch (Exception ex) 
                    {
                        // TODO: Log/Retry
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
