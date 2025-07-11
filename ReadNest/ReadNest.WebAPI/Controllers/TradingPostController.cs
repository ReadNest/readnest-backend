﻿using System.Net;
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
        public async Task<IActionResult> GetTradingPostsByUserId([FromQuery] GetTradingPostPagingRequest request)
        {
            var response = await _tradingPostUseCase.GetTradingPostByUserIdAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("v2")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookTradingPostV2Response>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTradingPosts([FromQuery] PagingRequest request)
        {
            var response = await _tradingPostUseCase.GetTradingPostv2Async(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("top")]
        [ProducesResponseType(typeof(ApiResponse<List<GetBookTradingPostV2Response>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTradingPosts([FromQuery] int? limit = 5)
        {
            var response = await _tradingPostUseCase.GetTopTradingPostsAsync(limit);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("{tradingPostId}/trading-requests")]
        [ProducesResponseType(typeof(ApiResponse<List<GetUserRequestResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetTradingRequestsByTradingPostId([FromRoute] Guid tradingPostId)
        {
            var response = await _tradingPostUseCase.GetUserRequestsByIdAsync(tradingPostId);
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

        [HttpPost("v2")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateTradingPostV2([FromBody] CreateTradingPostRequestV2 request)
        {
            var response = await _tradingPostUseCase.CreateTradingPostV2Async(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{tradingPostId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateTradingPost([FromRoute] Guid tradingPostId, [FromBody] UpdateTradingPostRequest request)
        {
            var response = await _tradingPostUseCase.UpdateTradingPostAsync(tradingPostId, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPatch("{tradingPostId}/trading-requests/{tradingRequestId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateStatusTradingRequest([FromRoute] Guid tradingPostId, [FromRoute] Guid tradingRequestId, [FromBody] UpdateStatusTradingRequest request)
        {
            var response = await _tradingPostUseCase.UpdateStatusTradingRequestAsync(tradingPostId, tradingRequestId, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteTradingPost([FromRoute] Guid id)
        {
            var response = await _tradingPostUseCase.DeleteTradingPostAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
