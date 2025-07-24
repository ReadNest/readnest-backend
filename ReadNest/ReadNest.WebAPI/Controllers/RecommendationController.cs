using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.UseCases.Interfaces.Recommendation;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/recommendations")]
    [ApiController]
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

        [HttpGet("/api/v1/user/{userId:guid}/recommendations")]
        public async Task<IActionResult> GetRecommendationBooksAsync([FromRoute] Guid userId)
        {
            return Ok(await _useCase.RecommendBooksAsync(userId));
        }
    }
}
