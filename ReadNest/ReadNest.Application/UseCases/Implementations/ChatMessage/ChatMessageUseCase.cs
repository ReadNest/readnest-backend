using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Models.Responses.ChatMessage;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.ChatMessage;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.ChatMessage
{
    public class ChatMessageUseCase : IChatMessageUseCase
    {
        private readonly IChatMessageRepository _chatMessageRepository;

        public ChatMessageUseCase(IChatMessageRepository chatMessageRepository)
        {
            _chatMessageRepository = chatMessageRepository;
        }

        public async Task<ApiResponse<List<RecentChatterResponse>>> GetAllChattersByUserIdAsync(Guid id)
        {
            var chatter = await _chatMessageRepository.GetAllChattersByUserIdAsync(id);

            if (chatter == null || !chatter.Any())
            {
                return new ApiResponse<List<RecentChatterResponse>>
                {
                    Success = true,
                    Message = "No chatters found for the specified user.",
                    Data = new List<RecentChatterResponse>()
                };
            }
            var response = chatter.Select(c => new RecentChatterResponse
            {
                UserId = c.Id,
                UserName = c.UserName,
                FullName = c.FullName,
                AvatarUrl = c.AvatarUrl,
                LastMessage = c.SentMessages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.Message,
                LastMessageTime = c.SentMessages.OrderByDescending(m => m.SentAt).FirstOrDefault()?.SentAt ?? DateTime.MinValue,
                UnreadMessagesCount = c.SentMessages.Count(m => m.ReceiverId == id && !m.IsRead)
            }).ToList();
            return new ApiResponse<List<RecentChatterResponse>>
            {
                Success = true,
                Message = "Chatters retrieved successfully.",
                Data = response
            };
        }

        public async Task<ApiResponse<List<ChatMessageCacheModel>>> GetFullConversationAsync(Guid userAId, Guid userBId)
        {
            var conversation = await _chatMessageRepository.GetFullConversationAsync(userAId, userBId);
            if (conversation == null || !conversation.Any())
            {
                return new ApiResponse<List<ChatMessageCacheModel>>
                {
                    Success = true,
                    Message = "No conversation found between the specified users.",
                    Data = new List<ChatMessageCacheModel>()
                };
            }
            else
            {
                var conversationModel = conversation.Select(m => new ChatMessageCacheModel
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Message = m.Message,
                    SentAt = m.SentAt,
                    IsRead = m.IsRead,
                    IsSaved = true // true = from DB; false = new pending
                }).ToList();
                return new ApiResponse<List<ChatMessageCacheModel>>
                {
                    Success = true,
                    Message = "Conversation retrieved successfully.",
                    Data = conversationModel
                };
            }
        }

        public async Task<ApiResponse<string>> SaveRangeMessageAsync(List<Domain.Entities.ChatMessage> messages)
        {
            if (messages == null || !messages.Any())
            {
                return await Task.FromResult(new ApiResponse<string>
                {
                    Success = false,
                    Message = "No messages to save."
                });
            }
            try
            {
                await _chatMessageRepository.AddRangeAsync(messages);
                await _chatMessageRepository.SaveChangesAsync();
                return await Task.FromResult(new ApiResponse<string>
                {
                    Success = true,
                    Message = "Messages saved successfully."
                });
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ApiResponse<string>
                {
                    Success = false,
                    Message = $"Error saving messages: {ex.Message}"
                });
            }
        }
    }
}
