using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IUserRepository : IGenericRepository<User, Guid>
    {
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUserNameAsync(string username);
        Task<User?> LoginAsync(string username, string password);
        Task<User?> GetByUserIdAsync(Guid userId);
        Task<User> GetByUserNameAsync(string userName);
        Task<IEnumerable<User>> GetAllUsersWithRoleUserAsync();
        Task<bool> isActivePremium(Guid userId);
    }
}
