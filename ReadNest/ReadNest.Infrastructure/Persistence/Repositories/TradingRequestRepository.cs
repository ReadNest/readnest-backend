using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class TradingRequestRepository(AppDbContext context) : GenericRepository<TradingRequest, Guid>(context), ITradingRequestRepository
    {
    }
}
