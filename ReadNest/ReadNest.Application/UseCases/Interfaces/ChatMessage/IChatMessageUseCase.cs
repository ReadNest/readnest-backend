using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Models.Responses.ChatMessage;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.ChatMessage
{
    public interface IChatMessageUseCase
    {
        Task<ApiResponse<String>> SaveRangeMessageAsync(List<Domain.Entities.ChatMessage> messages);
        Task<ApiResponse<List<RecentChatterResponse>>> GetAllChattersByUserIdAsync(Guid id);
        Task<ApiResponse<List<ChatMessageCacheModel>>> GetFullConversationAsync(Guid userAId, Guid userBId);
        Task<ApiResponse<RecentChatterResponse>> GetUserWhoSentMessageToAsync(Guid receiverId, string senderUsername);
        Task<ApiResponse<RecentChatterResponse>> GetUserWhoSendMessageToByIdAsync(Guid senderId, Guid receiverId);
    }
}
