using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class PostRepository(AppDbContext context) : GenericRepository<Post, Guid>(context), IPostRepository
    {
        public async Task<IEnumerable<Post>> GetAllPostsAsync()
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Book)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        public async Task<Post> GetPostWithLikesByIdAsync(Guid postId)
        {
            return await _context.Posts
                .Include(p => p.Likes)
                .FirstOrDefaultAsync(p => p.Id == postId && !p.IsDeleted);
        }

        public async Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .Include(p => p.Book)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<int> GetPostCountByUserIdAsync(Guid userId)
        {
            return await _context.Posts
                .CountAsync(p => p.UserId == userId && !p.IsDeleted);
        }

        public async Task<IEnumerable<Post>> GetPostsByBookIdAsync(Guid bookId)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => p.BookId == bookId && !p.IsDeleted)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetTopMostLikedPostsAsync(int count)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.Likes.Count())
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetTopMostViewedPostsAsync(int count)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .OrderByDescending(p => p.Views)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> SearchByTitleAsync(string keyword)
        {
            return await _context.Posts
                .AsNoTracking()
                .Where(p => !p.IsDeleted && EF.Functions.Like(p.Title, $"%{keyword}%"))
                .ToListAsync();
        }

        public IQueryable<Post> GetQueryableWithIncludes()
        {
            return _context.Posts
                .Include(p => p.Book)
                .Include(p => p.Creator)
                .Include(p => p.Likes)
                .Where(p => !p.IsDeleted);
        }
    }
}
