using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.UseCases.Interfaces.UserBadge;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserBadgesController : ControllerBase
    {
        private readonly IUserBadgeUseCase _userBadgeUseCase;
        public UserBadgesController(IUserBadgeUseCase userBadgeUseCase)
        {
            _userBadgeUseCase = userBadgeUseCase;
        }

        [HttpPost("assign-badge-to-all-users")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AssignBadgeToAllUsers(string badgeCode)
        {
            var response = await _userBadgeUseCase.AssignBadgeToAllUsers(badgeCode);
            return Ok(response);
        }
        [HttpPost("set-all-badges-active")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SelectUserBadge(string badgeCode)
        {
            var response = await _userBadgeUseCase.SetAllBadgesActiveByBadgeCode(badgeCode);
            return Ok(response);
        }
        [HttpPost("select-user-badge")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SelectUserBadge(Guid userId, Guid badgeId)
        {
            var response = await _userBadgeUseCase.SelectUserBadge(userId, badgeId);
            return Ok(response);
        }
    }
}
