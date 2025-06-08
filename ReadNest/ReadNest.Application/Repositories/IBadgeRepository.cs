using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IBadgeRepository : IGenericRepository<Badge, Guid>
    {
        public Task<Badge?> GetByCodeAsync(string code);
        public Task<IEnumerable<Badge>> GetAllAvailableAsync();
    }
}
