using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.UseCases.Interfaces.Recommendation;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/recommendations")]
    [ApiController]
    public class RecommendationController : ControllerBase
    {
        private readonly IRecommendationUseCase _useCase;

        public RecommendationController(IRecommendationUseCase useCase)
        {
            _useCase = useCase;
        }

        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> GetRecommendationBooksAsync([FromRoute] Guid userId)
        {
            return Ok(await _useCase.RecommendBooksAsync(userId));
        }
    }
}
