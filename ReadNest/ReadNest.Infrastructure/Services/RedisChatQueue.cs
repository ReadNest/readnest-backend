using System.Text.Json;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Services;
using ReadNest.Domain.Entities;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisChatQueue : IRedisChatQueue
    {
        private string GetChatKey(Guid senderId, Guid receiverId)
        {
            return senderId.CompareTo(receiverId) < 0 ? $"chat:{senderId}:{receiverId}" : $"chat:{receiverId}:{senderId}";
        }
        private string PendingChatKey = "chat:pending"; // Key for pending messages, if needed
        private readonly IDatabase _redisDb;
        /// <summary>
        /// Cache Time To Live (TTL) for chat messages in Redis.
        /// </summary>
        private readonly TimeSpan _cacheTTL = TimeSpan.FromMinutes(3);
        public RedisChatQueue(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }
        /// <summary>
        /// Adds a message to the Redis cache for a chat between two users.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isSaved"></param>
        /// <returns></returns>
        public async Task AddMessageAsync(ChatMessageCacheModel message, bool isSaved)
        {
            message.SentAt = DateTime.UtcNow; // Ensure the message has a timestamp
            message.IsRead = false; // Default to unread
            message.IsSaved = isSaved; // Set the IsSaved property based on the input

            var key = GetChatKey(message.SenderId, message.ReceiverId);
            // Flag to check if the key already exists
            bool keyExists = await _redisDb.KeyExistsAsync(key);
            var score = new DateTimeOffset(message.SentAt).ToUnixTimeMilliseconds();
            await _redisDb.SortedSetAddAsync(key, JsonSerializer.Serialize(message), score);
            // TTL chỉ set khi TẠO KEY MỚI. Nếu key đã tồn tại → không reset TTL.
            if (!keyExists)
            {
                await _redisDb.KeyExpireAsync(key, _cacheTTL);
            }

            if (!isSaved)
            {
                // If the message is saved, we can also add it to a pending messages set if needed
                await _redisDb.ListRightPushAsync(PendingChatKey, JsonSerializer.Serialize(message));
                bool pendingKeyExists = await _redisDb.KeyExistsAsync(PendingChatKey);
                if (!pendingKeyExists)
                {
                    await _redisDb.KeyExpireAsync(PendingChatKey, _cacheTTL);
                }
            }

        }
        /// <summary>
        /// Retrieves the full conversation between two users (both directions) from the Redis cache.
        /// </summary>
        /// <param name="userAId"></param>
        /// <param name="userBId"></param>
        /// returns>A list of chat messages sorted by sent time.</returns>
        public async Task<List<ChatMessageCacheModel>> GetFullConversationDequeueAsync(Guid userAId, Guid userBId)
        {
            var keyAB = GetChatKey(userAId, userBId);
            //var keyBA = GetChatKey(userBId, userAId);

            var redisMessagesAB = await _redisDb.SortedSetRangeByRankAsync(keyAB);
            //var redisMessagesBA = await _redisDb.SortedSetRangeByRankAsync(keyBA);

            var messagesAB = redisMessagesAB
                .Select(m => JsonSerializer.Deserialize<ChatMessageCacheModel>(m!)!)
                .ToList();

            //var messagesBA = redisMessagesBA
            //    .Select(m => JsonSerializer.Deserialize<ChatMessageCacheModel>(m!)!)
            //    .ToList();

            //var fullConversation = messagesAB.Concat(messagesBA)
            //    .OrderBy(m => m.SentAt)
            //    .ToList();
            var fullConversation = messagesAB
                .OrderBy(m => m.SentAt)
                .ToList();

            return fullConversation;
        }
        /// <summary>
        /// Retrieves pending message from the Redis cache.
        /// </summary>
        /// <returns></returns>
        public async Task<string?> DequeuePendingMessageAsync()
        {
            return await _redisDb.ListLeftPopAsync(PendingChatKey);
        }

        public async Task ClearMessagesAsync(Guid senderId, Guid receiverId)
        {
            await _redisDb.KeyDeleteAsync(GetChatKey(senderId, receiverId));
        }
        public async Task ClearPendingMessagesAsync()
        {
            await _redisDb.KeyDeleteAsync(PendingChatKey);
        }

        public async Task RefreshConversationCacheAsync(Guid senderId, Guid receiverId, List<ChatMessageCacheModel> dbMessages)
        {
            var key = GetChatKey(senderId, receiverId);

            await _redisDb.KeyDeleteAsync(key); // XÓA dữ liệu cũ → tránh duplicated

            foreach (var message in dbMessages)
            {
                var score = new DateTimeOffset(message.SentAt).ToUnixTimeMilliseconds();
                await _redisDb.SortedSetAddAsync(key, JsonSerializer.Serialize(message), score);
            }

            await _redisDb.KeyExpireAsync(key, _cacheTTL); // Reset TTL sau khi lưu mới
        }
    }
}
