using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class LeaderboardRepository(AppDbContext context) : GenericRepository<Leaderboard, Guid>(context), ILeaderboardRepository
    {
        public async Task<IEnumerable<Leaderboard>> GetTopNAsync(Guid eventId, int top)
        {
            return await _context.Leaderboards
                .Where(l => l.EventId == eventId && !l.IsDeleted)
                .Include(l => l.User)
                .OrderByDescending(l => l.Score)
                .Take(top)
                .ToListAsync();
        }

        public async Task<Leaderboard?> GetUserLeaderboardAsync(Guid eventId, Guid userId)
        {
            return await _context.Leaderboards
                .FirstOrDefaultAsync(l => l.EventId == eventId && l.UserId == userId && !l.IsDeleted);
        }

        public async Task<IEnumerable<Leaderboard>> GetTopByTimeRangeAsync(DateTime from, DateTime to, int top)
        {
            return await _context.Leaderboards
                .Where(l => l.CreatedAt >= from && l.CreatedAt <= to && !l.IsDeleted)
                .OrderByDescending(l => l.Score)
                .Take(top)
                .Include(l => l.User)
                .ToListAsync();
        }

        public async Task<int?> GetUserRankAsync(Guid eventId, Guid userId)
        {
            var leaderboard = await _context.Leaderboards
                .Where(l => l.EventId == eventId && l.UserId == userId && !l.IsDeleted)
                .Select(l => new { l.Score, l.Rank })
                .FirstOrDefaultAsync();

            return leaderboard?.Rank;
        }
    }
}
