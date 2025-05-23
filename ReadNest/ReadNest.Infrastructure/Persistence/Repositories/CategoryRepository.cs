using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, Guid>(context), ICategoryRepository
    {
    }
}
