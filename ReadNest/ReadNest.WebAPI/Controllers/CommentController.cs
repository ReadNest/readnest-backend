using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Comment;
using ReadNest.Application.Models.Requests.CommentLike;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Application.UseCases.Interfaces.Comment;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentUseCase _commentUseCase;

        public CommentController(ICommentUseCase commentUseCase)
        {
            _commentUseCase = commentUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetCommentResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var response = await _commentUseCase.CreateAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetCommentResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPublishedCommentsByBookId(Guid bookId)
        {
            var response = await _commentUseCase.GetPublishedCommentsByBookIdAsync(bookId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost("like")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LikeComment([FromBody] CreateCommentLikeRequest request)
        {
            var response = await _commentUseCase.LikeCommentAsync(request.CommentId, request.UserId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        // Update Comment content
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentRequest request)
        {
            var response = await _commentUseCase.UpdateCommentAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }
        // Soft Delete Comment
        [HttpDelete("{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var response = await _commentUseCase.DeleteCommentAsync(commentId);
            return response.Success ? Ok(response) : NotFound(response);
        }
        // Report Comment
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ReportComment([FromBody] ReportCommentRequest request)
        {
            var response = await _commentUseCase.ReportCommentAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
