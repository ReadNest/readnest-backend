using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IUserBadgeRepository : IGenericRepository<UserBadge, Guid>
    {
        public Task<IEnumerable<UserBadge>> GetAvailableByUserIdAsync(Guid userId);
        public Task<IEnumerable<UserBadge>> GetByBadgeIdAsync(Guid badgeId);
    }
}
