using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

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
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email) != null;
        }

        public async Task<bool> ExistsByUserNameAsync(string username)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.UserName == username) != null;
        }

        public async Task<User?> LoginAsync(string username, string password)
        {
            var user = await _context.Users.Include(x => x.Role).AsNoTracking().FirstOrDefaultAsync(x => x.UserName == username);
            var isSamePassword = BCrypt.Net.BCrypt.Verify(password, user?.HashPassword);
            if (user == null || !isSamePassword) return null;
            return user;
        }
    }
}
