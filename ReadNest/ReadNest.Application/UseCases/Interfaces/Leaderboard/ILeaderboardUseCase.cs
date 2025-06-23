using ReadNest.Application.Models.Responses.Leaderboard;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Leaderboard
{
    public interface ILeaderboardUseCase
    {
        Task<ApiResponse<IEnumerable<LeaderboardResponse>>> GetTopNAsync(Guid eventId, int top);
        Task<ApiResponse<LeaderboardResponse?>> GetUserLeaderboardAsync(Guid eventId, Guid userId);
        Task<ApiResponse<IEnumerable<LeaderboardResponse>>> GetTopByTimeRangeAsync(DateTime from, DateTime to, int top);
        Task<ApiResponse<LeaderboardRankResponse>> GetUserRankAsync(Guid eventId, Guid userId);

        Task<ApiResponse<string>> RecalculateLeaderboardScoresAsync(Guid eventId);
    }
}
