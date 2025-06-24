using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;
using ReadNest.Shared.Enums;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="context"></param>
    public class UserRepository(AppDbContext context) : GenericRepository<User, Guid>(context), IUserRepository
    {
        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted) != null;
        }

        public async Task<bool> ExistsByUserNameAsync(string username)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == username && !x.IsDeleted) != null;
        }

        public async Task<IEnumerable<User>> GetAllUsersWithRoleUserAsync()
        {
            return await _context.Users
                .Include(x => x.Role)
                .Where(x => x.Role.RoleName == RoleEnum.User.ToString() && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetByUserIdAsync(Guid userId)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Role).FirstOrDefaultAsync(x => x.Id == userId && !x.IsDeleted);
        }

        public async Task<User> GetByUserNameAsync(string userName)
        {
            var user = await _context.Users
                .Include(x => x.Role)
                .Include(x => x.Comments)
                .ThenInclude(c => c.Likes)
                .Include(x => x.Posts)
                .ThenInclude(p => p.Likes)
                .FirstOrDefaultAsync(x => x.UserName == userName && !x.IsDeleted);
            return user;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users.Include(x => x.Role).AsNoTracking().FirstOrDefaultAsync(x => x.UserName == username && !x.IsDeleted);
            if (user == null) return null;
            var isSamePassword = BCrypt.Net.BCrypt.Verify(password, user?.HashPassword);
            if (!isSamePassword) return null;

            return user;
        }
    }
}
