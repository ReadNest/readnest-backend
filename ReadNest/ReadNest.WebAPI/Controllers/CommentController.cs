using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Comment;
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
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            var response = await _commentUseCase.CreateAsync(request);
            return Ok(new { Message = "Comment created successfully" });
        }

        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetCommentResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPublishedCommentsByBookId(Guid bookId)
        {
            var response = await _commentUseCase.GetPublishedCommentsByBookIdAsync(bookId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
