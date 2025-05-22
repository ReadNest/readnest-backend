using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

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
    }
}
