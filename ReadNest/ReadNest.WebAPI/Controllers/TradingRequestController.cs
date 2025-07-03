using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.TradingPost;
using ReadNest.Application.UseCases.Interfaces.TradingPost;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/trading-requests")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class TradingRequestController : ControllerBase
    {
        private readonly ITradingPostUseCase _tradingPostUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tradingPostUseCase"></param>
        public TradingRequestController(ITradingPostUseCase tradingPostUseCase)
        {
            _tradingPostUseCase = tradingPostUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTradingRequest([FromBody] CreateTradingRequest request)
        {
            var response = await _tradingPostUseCase.CreateTradingRequestAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
