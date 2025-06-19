using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage, Guid>
    {
        Task<IEnumerable<User>> GetAllChattersByUserIdAsync(Guid userId);
    }
}
