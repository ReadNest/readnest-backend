using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ITradingRequestRepository : IGenericRepository<TradingRequest, Guid>
    {
    }
}
