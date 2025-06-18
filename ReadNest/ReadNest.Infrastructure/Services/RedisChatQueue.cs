using Newtonsoft.Json;
using ReadNest.Application.Services;
using ReadNest.Domain.Entities;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisChatQueue : IRedisChatQueue
    {
        private const string QueueKey = "chat:queue";
        private readonly IDatabase _redisDb;

        public RedisChatQueue(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        /// <summary>
        /// Enqueues a chat message to the Redis queue.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task EnqueueMessageAsync(ChatMessage message)
        {
            var json = JsonConvert.SerializeObject(message);
            await _redisDb.ListRightPushAsync(QueueKey, json);
        }
        /// <summary>
        /// Dequeues a chat message from the Redis queue.
        /// </summary>
        public async Task<string> DequeueRawMessageAsync()
        {
            return await _redisDb.ListLeftPopAsync(QueueKey);
        }
    }
}
