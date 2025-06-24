using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IEventRepository : IGenericRepository<Event, Guid>
    {
        Task<Event?> GetCurrentEventAsync();
        Task<IEnumerable<Event>> GetAllEventsAsync();
    }
}
