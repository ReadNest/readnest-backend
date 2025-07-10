using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class UserSubscriptionRepository(AppDbContext context) : GenericRepository<UserSubscription, Guid>(context), IUserSubscriptionRepository
    {
    }
}
