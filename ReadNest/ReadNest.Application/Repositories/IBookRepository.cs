using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IBookRepository : IGenericRepository<Book, Guid>
    {
    }
}
