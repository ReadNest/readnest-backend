using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Models.Responses.ChatMessage;

namespace ReadNest.Application.Services
{
    public interface IRedisChatQueue
    {
        Task AddMessageAsync(ChatMessageCacheModel message, bool isSaved);
        Task<List<ChatMessageCacheModel>> GetFullConversationFromCacheAsync(Guid userAId, Guid userBId);
        Task<string?> DequeuePendingMessageAsync();
        Task ClearMessagesAsync(Guid senderId, Guid receiverId);
        Task ClearPendingMessagesAsync();
        Task RefreshConversationCacheAsync(Guid senderId, Guid receiverId, List<ChatMessageCacheModel> dbMessages);
        Task CacheRecentChattersAsync(Guid userId, List<RecentChatterResponse> chatters);
        Task<List<RecentChatterResponse>?> GetCachedRecentChattersAsync(Guid userId);
        Task DeleteRecentChattersCacheAsync(Guid userId);
    }
}
