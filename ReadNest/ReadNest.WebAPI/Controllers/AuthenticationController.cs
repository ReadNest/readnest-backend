using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Auth;
using ReadNest.Application.Validators.Auth;
using ReadNest.WebAPI.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly RegisterRequestValidator _registerRequestValidator;

        public AuthenticationController(RegisterRequestValidator registerRequestValidator)
        {
            _registerRequestValidator = registerRequestValidator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _registerRequestValidator.ValidateAndThrowAsync(request);

            return Ok(new ApiResponse<string>
            {
                Success = true,
                MessageId = MessageId.I0000,
                Message = Message.GetMessageById(MessageId.I0000),
            });
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginRequest request) { }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request) { }

        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken(TokenRequest request) { }
    }
}
