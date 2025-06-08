using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.UseCases.Interfaces.UserBadge;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/[controller]")]
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
    }
}
