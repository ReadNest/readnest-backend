using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class EventRewardRepository(AppDbContext context) : GenericRepository<EventReward, Guid>(context), IEventRewardRepository
    {
        public async Task<IEnumerable<EventReward>> GetRewardsByEventIdAsync(Guid eventId)
        {
            return await _context.EventRewards
                .Where(r => r.EventId == eventId && !r.IsDeleted)
                .ToListAsync();
        }
    }
}
