using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class CategoryRepository(AppDbContext context) : GenericRepository<Category, Guid>(context), ICategoryRepository
    {
        public async Task<List<Category>> FindByIdsAsync(IEnumerable<Guid> bookIds)
        {
            return await _context.Categories.Where(c => bookIds.Contains(c.Id)).ToListAsync();
        }

        public async Task<List<Guid>> FindCategoryIdsByBookIdAsync(Guid bookId)
        {
            return await _context.Categories
                .Where(c => c.Books.Any(b => b.Id == bookId))
                .Select(c => c.Id)
                .ToListAsync();
        }
    }
}
