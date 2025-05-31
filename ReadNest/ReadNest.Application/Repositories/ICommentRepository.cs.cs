using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment, Guid>
    {
        public Task<IEnumerable<Comment>> GetPublishCommentsByBookIdAsync(Guid bookId);
        public Task<Comment> GetCommentWithLikesByIdAsync(Guid commentId);
        public Task<Comment> GetPublishCommentByIdAsync(Guid commentId);
        public Task<IEnumerable<Comment>> GetAllReportedCommentsAsync();
        public Task<IEnumerable<Comment>> GetTop3RecentCommentsByUserNameAsync(string userName);
        public Task<IEnumerable<Comment>> GetTop3MostLikedCommentsAsync();
    }
}
