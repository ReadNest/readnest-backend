using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class UserBadgeRepository(AppDbContext context) : GenericRepository<UserBadge, Guid>(context), IUserBadgeRepository
    {
        public async Task<IEnumerable<UserBadge>> GetAvailableByUserIdAsync(Guid userId)
        {
            return await _context.UserBadges
                .Include(ub => ub.Badge)
                .Where(ub => ub.UserId == userId && !ub.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserBadge>> GetByBadgeIdAsync(Guid badgeId)
        {
            return await _context.UserBadges
                .Include(ub => ub.User)
                .Where(ub => ub.BadgeId == badgeId && !ub.IsDeleted)
                .ToListAsync();
        }
    }
}
