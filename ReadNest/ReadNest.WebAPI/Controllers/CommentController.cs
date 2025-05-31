using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize(Roles = "Admin,User")]
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

        [HttpGet("pending-reported-comments")]
        [ProducesResponseType(typeof(ApiResponse<List<GetReportedCommentsResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPendingReportedComments()
        {
            var response = await _commentUseCase.GetAllPendingReportedCommentsAsync();
            return Ok(response);
        }
        [HttpGet("top-3-recent-comments/{userName}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetCommentResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTop3RecentCommentsByUserId(string userName)
        {
            var response = await _commentUseCase.GetTop3RecentCommentsByUserNameAsync(userName);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("top-3-most-liked-comments")]
        [ProducesResponseType(typeof(ApiResponse<List<GetCommentResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTop3MostLikedComments()
        {
            var response = await _commentUseCase.GetTop3MostLikedCommentsAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
