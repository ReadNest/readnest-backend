using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class BookRepository(AppDbContext context) : GenericRepository<Book, Guid>(context), IBookRepository
    {
    }
}
