
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class BadgeRepository(AppDbContext context) : GenericRepository<Badge, Guid>(context), IBadgeRepository
    {
        public async Task<Badge?> GetByCodeAsync(string code)
        {
            return await _context.Badges
                .Where(b => b.IsDeleted == false) // Ensure we only get non-deleted badges
                .FirstOrDefaultAsync(b => b.Code == code) ?? null;
        }
        public async Task<IEnumerable<Badge>> GetAllAvailableAsync()
        {
            return await _context.Badges
                .Where(b => b.IsDeleted == false) // Ensure we only get non-deleted badges
                .ToListAsync();
        }
    }
}
