using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Post;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/posts")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostController : ControllerBase
    {
        private readonly IPostUseCase _postUseCase;
        private readonly IViewTracker _viewTracker;

        public const string NameIdentifier = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";

        public PostController(IPostUseCase postUseCase, IViewTracker viewTracker)
        {
            _postUseCase = postUseCase;
            _viewTracker = viewTracker;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllPosts([FromQuery] PagingRequest request)
        {
            var response = await _postUseCase.GetAllPostsAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetPostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetPostsByUserId(Guid userId, [FromQuery] PagingRequest request)
        {
            var response = await _postUseCase.GetPostsByUserIdAsync(userId, request);
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

        [HttpPost("like")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> LikePost([FromBody] LikePostRequest request)
        {
            var response = await _postUseCase.LikePostAsync(request.PostId, request.UserId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<GetPostResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest request)
        {
            var response = await _postUseCase.UpdateAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{postId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeletePost(Guid postId)
        {
            var response = await _postUseCase.DeleteAsync(postId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("increase-views/{postId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> IncreasePostViews(Guid postId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Ok(ApiResponse<string>.Fail("User not found in token"));

            var redisKey = $"view:{userId}:{postId}";
            TimeSpan ttl = TimeSpan.FromHours(6);

            if (await _viewTracker.ShouldIncreaseViewAsync(redisKey, ttl))
            {
                var response = await _postUseCase.IncreasePostViewsAsync(postId);
                return response.Success ? Ok(response) : BadRequest(response);
            }

            return Ok(ApiResponse<string>.Ok("View already counted recently"));
        }

        [HttpPost("filter")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FilterPosts([FromBody] FilterPostRequest request)
        {
            var response = await _postUseCase.FilterPostsAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
