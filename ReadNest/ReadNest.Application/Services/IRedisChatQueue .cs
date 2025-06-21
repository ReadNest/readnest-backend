using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Services
{
    public interface IRedisChatQueue
    {
        Task AddMessageAsync(ChatMessageCacheModel message, bool isSaved);
        Task<List<ChatMessageCacheModel>> GetFullConversationDequeueAsync(Guid userAId, Guid userBId);
        Task<string> DequeuePendingMessageAsync();
        Task ClearMessagesAsync(Guid senderId, Guid receiverId);
        Task ClearPendingMessagesAsync();
        Task RefreshConversationCacheAsync(Guid senderId, Guid receiverId, List<ChatMessageCacheModel> dbMessages);
    }
}
