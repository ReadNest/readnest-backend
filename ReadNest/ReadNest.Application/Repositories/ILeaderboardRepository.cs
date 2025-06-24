using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ILeaderboardRepository : IGenericRepository<Leaderboard, Guid>
    {
        Task<IEnumerable<Leaderboard>> GetTopNAsync(Guid eventId, int top);
        Task<Leaderboard?> GetUserLeaderboardAsync(Guid eventId, Guid userId);
        Task<IEnumerable<Leaderboard>> GetTopByTimeRangeAsync(DateTime from, DateTime to, int top);


    }
}
