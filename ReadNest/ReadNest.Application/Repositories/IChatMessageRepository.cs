using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IChatMessageRepository : IGenericRepository<ChatMessage, Guid>
    {
    }
}
