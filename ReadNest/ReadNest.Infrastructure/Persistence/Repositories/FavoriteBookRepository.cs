using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;
using ReadNest.Shared.Common;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class FavoriteBookRepository(AppDbContext context) : GenericRepository<FavoriteBook, Guid>(context), IFavoriteBookRepository
    {
        public async Task<FavoriteBook?> GetByUserAndBookAsync(Guid userId, Guid bookId)
        {
            return await _context.FavoriteBooks
                .Include(fb => fb.User)
                .Include(fb => fb.Book)
                .FirstOrDefaultAsync(fb => fb.UserId == userId && fb.BookId == bookId);
        }

        public async Task<IEnumerable<Book>> GetFavoriteBooksByUserAsync(Guid userId)
        {
            return await _context.FavoriteBooks
                .Where(fb => fb.UserId == userId)
                .Select(fb => fb.Book)
                .ToListAsync();
        }

        public async Task<PagingResponse<Book>> GetFavoriteBooksByUserPagedAsync(Guid userId, int pageNumber, int pageSize)
        {
            var query = _context.FavoriteBooks
                                .Where(fb => fb.UserId == userId)
                                .Include(fb => fb.Book)
                                    .ThenInclude(b => b.Categories)
                                .AsNoTracking();

            var totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(fb => fb.Book.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(fb => fb.Book)
                .ToListAsync();

            return new PagingResponse<Book>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageNumber,
                PageSize = pageSize
            };
        }

    }
}
