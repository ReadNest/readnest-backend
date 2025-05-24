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
        public Task<IEnumerable<Comment>> GetPublishedCommentsByBookIdAsync(Guid bookId);
    }
}
