using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Auth;
using ReadNest.Application.UseCases.Interfaces.Auth;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/auth")]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationUseCase _authUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="authUseCase"></param>
        public AuthenticationController(IAuthenticationUseCase authUseCase)
        {
            _authUseCase = authUseCase;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var response = await _authUseCase.RegisterAsync(request);

            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authUseCase.LoginAsync(request);

            if (response.Success) return Ok(response);
            return Unauthorized(response);
        }

        //[HttpPost("forgot-password")]
        //public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request) { }

        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken(TokenRequest request) { }
    }
}
