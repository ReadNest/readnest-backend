using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UserController : ControllerBase
    {
        private readonly IUserUseCase _userUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userUseCase"></param>
        public UserController(IUserUseCase userUseCase)
        {
            _userUseCase = userUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetUserResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var response = await _userUseCase.GetAllAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<GetUserResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
        {
            var response = await _userUseCase.GetByIdAsync(userId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        //[HttpPost]
        //public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        //{

        //}

        //[HttpPut("{userId}")]
        //public Task<IActionResult> UpdateUserProfile([FromRoute] Guid userId, [FromBody] UpdateUserRequest request)
        //{

        //}

        [HttpDelete("{userId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteByUserId([FromRoute] Guid userId)
        {
            var response = await _userUseCase.DeleteAccountAsync(userId);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
