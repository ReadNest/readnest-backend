
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface IFavoriteBookRepository : IGenericRepository<FavoriteBook, Guid>
    {
        Task<FavoriteBook?> GetByUserAndBookAsync(Guid userId, Guid bookId);
        Task<IEnumerable<Book>> GetFavoriteBooksByUserAsync(Guid userId);
    }
}
