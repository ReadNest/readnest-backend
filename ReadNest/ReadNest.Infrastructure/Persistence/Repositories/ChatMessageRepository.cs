using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class ChatMessageRepository(AppDbContext context) : GenericRepository<ChatMessage, Guid>(context), IChatMessageRepository
    {
    }
}
