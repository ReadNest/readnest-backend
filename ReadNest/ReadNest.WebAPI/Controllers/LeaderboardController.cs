using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Responses.Leaderboard;
using ReadNest.Application.UseCases.Interfaces.Leaderboard;
using ReadNest.Shared.Common;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LeaderboardController : ControllerBase
    {
        private readonly ILeaderboardUseCase _leaderboardUseCase;

        public LeaderboardController(ILeaderboardUseCase leaderboardUseCase)
        {
            _leaderboardUseCase = leaderboardUseCase;
        }

        //[HttpGet("event/{eventId}")]
        //[ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaderboardResponse>>), (int)HttpStatusCode.OK)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> GetLeaderboardsByEventId(Guid eventId)
        //{
        //    var response = await _leaderboardUseCase.GetLeaderboardsByEventIdAsync(eventId);
        //    return response.Success ? Ok(response) : BadRequest(response);
        //}

        [HttpGet("user/{eventId}/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<LeaderboardResponse?>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserLeaderboard(Guid eventId, Guid userId)
        {
            var response = await _leaderboardUseCase.GetUserLeaderboardAsync(eventId, userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("top/{eventId}/{top}")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaderboardResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTopN(Guid eventId, int top)
        {
            var response = await _leaderboardUseCase.GetTopNAsync(eventId, top);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("rank/{eventId}/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<LeaderboardRankResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetUserRank(Guid eventId, Guid userId)
        {
            var response = await _leaderboardUseCase.GetUserRankAsync(eventId, userId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("top-by-time-range")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<LeaderboardResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetTopByTimeRange(DateTime from, DateTime to, int top)
        {
            var response = await _leaderboardUseCase.GetTopByTimeRangeAsync(from, to, top);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("recalculate-scores/{eventId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RecalculateLeaderboardScores(Guid eventId)
        {
            var response = await _leaderboardUseCase.RecalculateLeaderboardScoresAsync(eventId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
