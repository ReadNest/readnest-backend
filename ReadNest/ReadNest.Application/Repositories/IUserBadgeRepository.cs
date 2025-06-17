using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IUserBadgeRepository : IGenericRepository<UserBadge, Guid>
    {
        public Task<IEnumerable<UserBadge>> GetAvailableByUserIdAsync(Guid userId);
        public Task<IEnumerable<UserBadge>> GetByBadgeIdAsync(Guid badgeId);
        public Task<IEnumerable<UserBadge>> GetAvailableByBadgeCodeAsync(string badgeCode);
        public Task<UserBadge> GetSelectedBadgeByUserIdAsync(Guid userId);
    }
}
