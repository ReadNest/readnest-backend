
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.Repositories
{
    public interface IFavoriteBookRepository : IGenericRepository<FavoriteBook, Guid>
    {
        Task<FavoriteBook?> GetByUserAndBookAsync(Guid userId, Guid bookId);
        Task<PagingResponse<Book>> GetFavoriteBooksByUserPagedAsync(Guid userId, int pageNumber, int pageSize);
    }
}
