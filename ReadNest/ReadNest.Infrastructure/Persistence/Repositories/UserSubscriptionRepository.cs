using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;
using ReadNest.Shared.Enums;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class UserSubscriptionRepository(AppDbContext context) : GenericRepository<UserSubscription, Guid>(context), IUserSubscriptionRepository
    {
        public async Task<UserSubscription> GetActiveSubscriptionByUserIdAsync(Guid userId)
        {
            return await _context.UserSubscriptions
                .AsNoTracking()
                .Where(s => s.UserId == userId && s.Status == StatusEnum.Active.ToString())
                .OrderByDescending(s => s.EndDate ?? DateTime.MaxValue)
                .FirstOrDefaultAsync();
        }
    }
}
