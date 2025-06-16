using ReadNest.Application.Services;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisViewTracker : IViewTracker
    {
        private readonly IDatabase _redis;
        private const string KeyPrefix = "view:";

        public RedisViewTracker(IConnectionMultiplexer redis)
        {
            _redis = redis.GetDatabase();
        }

        public async Task<bool> ShouldIncreaseViewAsync(string key, TimeSpan ttl)
        {
            try
            {
                var fullKey = $"{KeyPrefix}{key}";
                var success = await _redis.StringSetAsync(fullKey, "1", ttl, when: When.NotExists);
                return success;
            }
            catch (Exception ex)
            {
                // Optional: log nếu cần
                Console.WriteLine($"Redis error: {ex.Message}");
                return true; // fallback cho phép tăng view nếu Redis lỗi
            }
        }
    }
}
