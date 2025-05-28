using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/posts")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : Controller
    {
        private readonly IPostUseCase _postUseCase;

        public PostController(IPostUseCase postUseCase)
        {
            _postUseCase = postUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllPosts()
        {
            var response = await _postUseCase.GetAllPostsAsync();
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetPostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var response = await _postUseCase.CreateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<GetPostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            var response = await _postUseCase.GetPostByIdAsync(postId);
            return response.Success ? Ok(response) : NotFound(response);
        }
        
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostsByUserId(Guid userId)
        {
            var response = await _postUseCase.GetPostsByUserIdAsync(userId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("book/{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetPostsByBookId(Guid bookId)
        {
            var response = await _postUseCase.GetPostsByBookIdAsync(bookId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("top-liked/{count}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTopMostLikedPosts(int count)
        {
            var response = await _postUseCase.GetTopMostLikedPostsAsync(count);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("top-viewed/{count}")]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTopMostViewedPosts(int count)
        {
            var response = await _postUseCase.GetTopMostViewedPostsAsync(count);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<List<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> SearchByTitle([FromQuery] string keyword)
        {
            var response = await _postUseCase.SearchByTitleAsync(keyword);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost("like")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> LikePost([FromBody] LikePostRequest request)
        {
            var response = await _postUseCase.LikePostAsync(request.PostId, request.UserId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

    }
}
