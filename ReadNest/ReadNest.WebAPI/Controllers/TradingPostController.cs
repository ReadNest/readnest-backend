using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.TradingPost;
using ReadNest.Application.Models.Responses.TradingPost;
using ReadNest.Application.UseCases.Interfaces.TradingPost;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/trading-posts")]
    [ApiController]
    public class TradingPostController : ControllerBase
    {
        private readonly ITradingPostUseCase _tradingPostUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="tradingPostUseCase"></param>
        public TradingPostController(ITradingPostUseCase tradingPostUseCase)
        {
            _tradingPostUseCase = tradingPostUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookTradingPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTradingPostByUserId([FromRoute] GetTradingPostPagingRequest request)
        {
            var response = await _tradingPostUseCase.GetTradingPostByUserIdAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTradingPost([FromBody] CreateTradingPostRequest request)
        {
            var response = await _tradingPostUseCase.CreateTradingPostAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
