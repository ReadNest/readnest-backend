using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription, Guid>
    {
        Task<UserSubscription> GetActiveSubscriptionByUserIdAsync(Guid userId);
    }
}
