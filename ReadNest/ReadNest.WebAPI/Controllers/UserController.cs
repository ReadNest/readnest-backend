using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.UseCases.Interfaces.User;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var response = await _userUseCase.GetAllAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId([FromQuery] Guid userId)
        {
            var response = await _userUseCase.GetByIdAsync(userId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        //[HttpPost]
        //public Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        //{

        //}

        //[HttpPut("{userId}")]
        //public Task<IActionResult> UpdateUserProfile([FromQuery] Guid userId, [FromBody] UpdateUserRequest request)
        //{

        //}

        //[HttpDelete("{userId}")]
        //public Task<IActionResult> DeleteByUserId([FromQuery] Guid userId)
        //{

        //}
    }
}
