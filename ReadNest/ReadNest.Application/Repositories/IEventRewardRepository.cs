using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IEventRewardRepository : IGenericRepository<EventReward, Guid>
    {
        Task<IEnumerable<EventReward>> GetRewardsByEventIdAsync(Guid eventId);
    }
}
