using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.ChatMessage;
using ReadNest.Application.Models.Responses.ChatMessage;
using ReadNest.Application.UseCases.Interfaces.ChatMessage;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ChatMessagesController : ControllerBase
    {
        private readonly IChatMessageUseCase _chatMessageUseCase;
        public ChatMessagesController(IChatMessageUseCase chatMessageUseCase)
        {
            _chatMessageUseCase = chatMessageUseCase;
        }
        [HttpGet("get-all-chatters-by-user-id/{id}")]
        [ProducesResponseType(typeof(ApiResponse<RecentChatterResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllChattersByUserIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("User ID cannot be empty.");
            }
            var response = await _chatMessageUseCase.GetAllChattersByUserIdAsync(id);
            // This method should call the ChatMessageUseCase to get all chatters by user ID.
            // For now, we return a placeholder response.
            return Ok(response);
        }

        [HttpGet("get-full-conversation/{userAId}/{userBId}")]
        [ProducesResponseType(typeof(ApiResponse<List<ChatMessageCacheModel>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetFullConversationAsync(Guid userAId, Guid userBId)
        {
            if (userAId == Guid.Empty || userBId == Guid.Empty)
            {
                return BadRequest("User IDs cannot be empty.");
            }
            var response = await _chatMessageUseCase.GetFullConversationAsync(userAId, userBId);
            return Ok(response);
        }
    }
}
