using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Responses.UserSubscription;
using ReadNest.Application.UseCases.Interfaces.UserSubscription;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/user-subscriptions")]
    [ApiController]
    public class UserSubscriptionController : ControllerBase
    {
        private readonly IUserSubscriptionUseCase _userSubscriptionUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSubscriptionUseCase"></param>
        public UserSubscriptionController(IUserSubscriptionUseCase userSubscriptionUseCase)
        {
            _userSubscriptionUseCase = userSubscriptionUseCase;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserSubscriptionResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserSubscriptionByUserId([FromRoute] Guid userId)
        {
            var response = await _userSubscriptionUseCase.GetUserSubscriptionByUserIdAsync(userId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
