using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Badge;
using ReadNest.Application.Models.Responses.Badge;
using ReadNest.Application.UseCases.Interfaces.Badge;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BadgeController : ControllerBase
    {
        private readonly IBadgeUseCase _badgeUseCase;
        public BadgeController(IBadgeUseCase badgeUseCase)
        {
            _badgeUseCase = badgeUseCase;
        }
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<GetBadgeResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetBadges()
        {
            var response = await _badgeUseCase.GetBadgesAsync();
            return Ok(response);
        }
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<CreateBadgeResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateBadge(CreateBadgeRequest request)
        {
            var response = await _badgeUseCase.CreateBadgeAsync(request);
            return Ok(response);
        }

        [HttpDelete("{code}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SoftDeleteBadgeByCode(string code)
        {
            var response = await _badgeUseCase.SoftDeleteBadgeByCodeAsync(code);
            return Ok(response);
        }
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateBadge(UpdateBadgeRequest request)
        {
            var response = await _badgeUseCase.UpdateBadgeAsync(request);
            return Ok(response);
        }
    }
}
