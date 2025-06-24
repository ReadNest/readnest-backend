using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class TradingPostRepository(AppDbContext context) : GenericRepository<TradingPost, Guid>(context), ITradingPostRepository
    {
    }
}
