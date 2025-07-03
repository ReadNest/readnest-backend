using System.Text.Json;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Models.Responses.ChatMessage;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using StackExchange.Redis;

namespace ReadNest.Infrastructure.Services
{
    public class RedisChatQueue : IRedisChatQueue
    {
        private string GetRecentChattersKey(Guid userId) => $"recentChatters:{userId}";
        private string GetChatKey(Guid senderId, Guid receiverId)
        {
            return senderId.CompareTo(receiverId) < 0 ? $"chat:{senderId}:{receiverId}" : $"chat:{receiverId}:{senderId}";
        }
        private readonly string PendingChatKey = "chat:pending"; // Key for pending messages, if needed
        private readonly IDatabase _redisDb;
        /// <summary>
        /// Cache Time To Live (TTL) for chat messages in Redis.
        /// </summary>
        private readonly TimeSpan _cacheTTL = TimeSpan.FromMinutes(3);
        private readonly IUserRepository _userRepository;

        public RedisChatQueue(IConnectionMultiplexer redis, IUserRepository userRepository)
        {
            _redisDb = redis.GetDatabase();
            _userRepository = userRepository;
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
            _ = await _redisDb.SortedSetAddAsync(key, JsonSerializer.Serialize(message), score);
            // TTL chỉ set khi TẠO KEY MỚI. Nếu key đã tồn tại → không reset TTL.
            if (!keyExists)
            {
                _ = await _redisDb.KeyExpireAsync(key, _cacheTTL);
            }

            if (!isSaved)
            {
                // If the message is saved, we can also add it to a pending messages set if needed
                _ = await _redisDb.ListRightPushAsync(PendingChatKey, JsonSerializer.Serialize(message));
                _ = await _redisDb.KeyExpireAsync(PendingChatKey, _cacheTTL); // luôn reset TTL mỗi lần push
            }

            // Cập nhật recentChatters của cả sender và receiver
            await AddOrUpdateRecentChatterAsync(message.SenderId, message.ReceiverId, message.Message, message.SentAt);
            await AddOrUpdateRecentChatterAsync(message.ReceiverId, message.SenderId, message.Message, message.SentAt);

        }
        /// <summary>
        /// Retrieves the full conversation between two users (both directions) from the Redis cache.
        /// </summary>
        /// <param name="userAId"></param>
        /// <param name="userBId"></param>
        /// returns>A list of chat messages sorted by sent time.</returns>
        public async Task<List<ChatMessageCacheModel>> GetFullConversationFromCacheAsync(Guid userAId, Guid userBId)
        {
            var keyAB = GetChatKey(userAId, userBId);
            //var keyBA = GetChatKey(userBId, userAId);

            var redisMessagesAB = await _redisDb.SortedSetRangeByRankAsync(keyAB);
            //var redisMessagesBA = await _redisDb.SortedSetRangeByRankAsync(keyBA);

            var messagesAB = redisMessagesAB
                .Select(m => JsonSerializer.Deserialize<ChatMessageCacheModel>(m!)!)
                .ToList();

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
            _ = await _redisDb.KeyDeleteAsync(GetChatKey(senderId, receiverId));
        }
        public async Task ClearPendingMessagesAsync()
        {
            _ = await _redisDb.KeyDeleteAsync(PendingChatKey);
        }

        public async Task RefreshConversationCacheAsync(Guid senderId, Guid receiverId, List<ChatMessageCacheModel> dbMessages)
        {
            var key = GetChatKey(senderId, receiverId);

            _ = await _redisDb.KeyDeleteAsync(key); // XÓA dữ liệu cũ → tránh duplicated

            foreach (var message in dbMessages)
            {
                var score = new DateTimeOffset(message.SentAt).ToUnixTimeMilliseconds();
                _ = await _redisDb.SortedSetAddAsync(key, JsonSerializer.Serialize(message), score);
            }

            _ = await _redisDb.KeyExpireAsync(key, _cacheTTL); // Reset TTL sau khi lưu mới
        }

        public async Task CacheRecentChattersAsync(Guid userId, List<RecentChatterResponse> chatters)
        {
            var key = GetRecentChattersKey(userId);
            var serialized = JsonSerializer.Serialize(chatters);
            await _redisDb.StringSetAsync(key, serialized, TimeSpan.FromMinutes(5));
        }

        public async Task<List<RecentChatterResponse>?> GetCachedRecentChattersAsync(Guid userId)
        {
            var key = GetRecentChattersKey(userId);
            var cached = await _redisDb.StringGetAsync(key);
            if (cached.IsNullOrEmpty) return null;

            return JsonSerializer.Deserialize<List<RecentChatterResponse>>(cached!);
        }
        public async Task DeleteRecentChattersCacheAsync(Guid userId)
        {
            var key = $"recentChatters:{userId}";
            await _redisDb.KeyDeleteAsync(key);
        }
        private async Task AddOrUpdateRecentChatterAsync(Guid ownerId, Guid chatterId, string lastMessage, DateTime sentAt)
        {
            var key = GetRecentChattersKey(ownerId);
            var cached = await _redisDb.StringGetAsync(key);

            List<RecentChatterResponse> chatters = new();

            // Nếu cache đã tồn tại, deserialize danh sách chatters
            if (!cached.IsNullOrEmpty)
            {
                chatters = JsonSerializer.Deserialize<List<RecentChatterResponse>>(cached!)!;
            }
            // Tìm chatter đã tồn tại trong danh sách
            var existing = chatters.FirstOrDefault(c => c.UserId == chatterId);

            // Nếu đã tồn tại, cập nhật thông tin
            if (existing != null)
            {
                existing.LastMessage = lastMessage;
                existing.LastMessageTime = sentAt;
                // Xóa chatter cũ khỏi danh sách
                chatters.RemoveAll(c => c.UserId == chatterId);
                // Thêm chatter đã cập nhật vào đầu danh sách
                chatters.Insert(0, existing);
            }
            // Nếu chưa tồn tại, tạo mới
            else
            {
                // Truy vấn DB lấy thông tin người dùng nếu chưa có
                var user = await _userRepository.GetByIdAsync(chatterId);
                // Tạo mới chatter
                var newChatter = new RecentChatterResponse
                {
                    UserId = chatterId,
                    UserName = user?.UserName ?? "",
                    FullName = user?.FullName ?? "",
                    AvatarUrl = user?.AvatarUrl ?? "",
                    LastMessage = lastMessage,
                    LastMessageTime = sentAt,
                    UnreadMessagesCount = 0
                };
                // Thêm chatter mới vào đầu danh sách
                chatters.Insert(0, newChatter);
            }
            // Cập nhật cache với danh sách chatters mới
            var updated = JsonSerializer.Serialize(chatters);
            // Đặt lại danh sách đã cập nhật vào Redis với thời gian sống (TTL) = 5
            await _redisDb.StringSetAsync(key, updated, TimeSpan.FromMinutes(5));
        }
    }
}
