using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.UseCases.Interfaces.Recommendation;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/recommendations")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationUseCase _useCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="useCase"></param>
        public RecommendationController(IRecommendationUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("{userId:guid}")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookSearchResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecommendationBooksAsync([FromRoute] Guid userId, [FromQuery] PagingRequest request)
        {
            return Ok(await _useCase.RecommendBooksAsync(userId, request));
        }

        [HttpPost("recommend")]
        [ProducesResponseType(typeof(ApiResponse<List<BookSuggestion>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RecommendBooks([FromBody] List<UserAnswer> answers)
        {   
            var books = await _useCase.RecommendBooksByGeminiAsync(answers);
            return Ok(books);
        }
    }
}
