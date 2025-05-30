using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Repositories;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Persistence.DBContext;

namespace ReadNest.Infrastructure.Persistence.Repositories
{
    public class CommentRepository(AppDbContext context) : GenericRepository<Comment, Guid>(context), ICommentRepository
    {
        public async Task<IEnumerable<Comment>> GetAllReportedCommentsAsync()
        {
            var comments = await _context.Comments
                .Include(c => c.Reports)
                .Where(c => c.Reports.Any(r => r.Status == "Pending") && !c.IsDeleted)
                .Include(c => c.Creator)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            // Sort list Reports bên trong mỗi comment
            foreach (var comment in comments)
            {
                comment.Reports = comment.Reports
                    .OrderByDescending(r => r.CreatedAt)
                    .ToList();
            }

            return comments;
        }

        public async Task<Comment> GetCommentWithLikesByIdAsync(Guid commentId)
        {
            return await _context.Comments
                .Include(c => c.Likes)
                .FirstOrDefaultAsync(c => c.Id == commentId && !c.IsDeleted);
        }

        public async Task<Comment> GetPublishCommentByIdAsync(Guid commentId)
        {
            return await _context.Comments
                .Where(c => c.Id == commentId && !c.IsDeleted && c.Status == "Published")
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Comment>> GetPublishCommentsByBookIdAsync(Guid bookId)
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.Reports)
                .Where(c => c.BookId == bookId && !c.IsDeleted && c.Status == "Published")
                .Include(c => c.Creator)
                .Include(c => c.Likes)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }
}
