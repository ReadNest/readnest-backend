using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category, Guid>
    {
    }
}
