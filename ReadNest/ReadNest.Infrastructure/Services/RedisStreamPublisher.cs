using System.Text.Json;
using ReadNest.Application.Services;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisStreamPublisher : IRedisPublisher
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisStreamPublisher(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task PublishInvoiceEmailEventAsync<T>(T eventData)
        {
            var db = _redis.GetDatabase();
            var json = JsonSerializer.Serialize(eventData);
            _ = await db.StreamAddAsync("email:invoice:stream", [
                new("data", json)
            ]);
        }
    }
}
