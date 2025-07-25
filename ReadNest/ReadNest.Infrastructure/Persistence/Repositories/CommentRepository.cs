﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<IEnumerable<Comment>> GetTop3RecentCommentsByUserNameAsync(string userName)
        {
            return await _context.Comments
                .AsNoTracking()
                .Include(c => c.Creator)
                .Where(c => c.Creator.UserName == userName && !c.IsDeleted && c.Status == "Published")
                .Include(c => c.Likes)
                .Include(c => c.Book)
                .OrderByDescending(c => c.CreatedAt)
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetTop3MostLikedCommentsAsync()
        {
            return await _context.Comments
                .AsNoTracking()
                .Where(c => !c.IsDeleted && c.Status == "Published")
                .Include(c => c.Creator)
                .Include(c => c.Likes)
                .Include(c => c.Book)
                .OrderByDescending(c => c.Likes.Count)
                .Take(3)
                .ToListAsync();
        }
    }
}
