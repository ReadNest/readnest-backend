using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class CommentRepository(AppDbContext context) : GenericRepository<Comment, Guid>(context), ICommentRepository
    {
        public async Task<IEnumerable<Comment>> GetPublishedCommentsByBookIdAsync(Guid bookId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => c.BookId == bookId && !c.IsDeleted && c.Status == "Published")
                .Include(c => c.Creator)
                .Include(c => c.Likes)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
