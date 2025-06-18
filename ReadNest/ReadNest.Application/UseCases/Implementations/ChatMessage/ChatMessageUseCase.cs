using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.ChatMessage;
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
