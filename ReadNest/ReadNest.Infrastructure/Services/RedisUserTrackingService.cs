using ReadNest.Application.Services;
using ReadNest.Domain.Entities;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisUserTrackingService : IRedisUserTrackingService
    {
        private readonly IDatabase _redis;
        private const string ClicksKeyPattern = "user:{0}:clicks";
        private const string KeywordsKeyPattern = "user:{0}:keywords";
        private const string GlobalKeywordsKey = "global:keywords";

        public RedisUserTrackingService(IConnectionMultiplexer redisConnection)
        {
            _redis = redisConnection.GetDatabase();
        }

        public async Task ClearUserTrackingAsync(Guid userId)
        {
            _ = await _redis.KeyDeleteAsync(string.Format(ClicksKeyPattern, userId));
            _ = await _redis.KeyDeleteAsync(string.Format(KeywordsKeyPattern, userId));
        }

        public async Task<List<Guid>> GetTopBookClicksAsync(Guid userId, int top = 5)
        {
            var key = string.Format(ClicksKeyPattern, userId);
            var results = await _redis.SortedSetRangeByRankWithScoresAsync(
                key, 0, top - 1, Order.Descending);

            return results.Select(r => Guid.Parse(r.Element)).ToList();
        }

        public async Task<List<string>> GetTopKeywordsAsync(Guid userId, int top = 5)
        {
            var key = string.Format(KeywordsKeyPattern, userId);
            var results = await _redis.SortedSetRangeByRankWithScoresAsync(
                key, 0, top - 1, Order.Descending);

            return results.Select(r => r.Element.ToString()).ToList();
        }

        public async Task TrackBookClickAsync(Guid userId, Guid bookId)
        {
            var key = string.Format(ClicksKeyPattern, userId);
            _ = await _redis.SortedSetIncrementAsync(key, bookId.ToString(), 1);
        }

        public async Task TrackKeywordSearchAsync(Guid userId, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            keyword = keyword.Trim().ToLower();
            var key = string.Format(KeywordsKeyPattern, userId);

            _ = await _redis.SortedSetIncrementAsync(key, keyword, 1);
            _ = await _redis.SortedSetIncrementAsync(GlobalKeywordsKey, keyword, 1);
        }

        public async Task<List<string>> GetTopGlobalKeywordsAsync(int top = 10)
        {
            var results = await _redis.SortedSetRangeByRankWithScoresAsync(
                GlobalKeywordsKey, 0, top - 1, Order.Descending);

            return results.Select(r => r.Element.ToString()).ToList();
        }
    }
}
