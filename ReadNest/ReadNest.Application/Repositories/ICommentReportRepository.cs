using ReadNest.Domain.Entities;

namespace ReadNest.Application.Repositories
{
    public interface ICommentReportRepository : IGenericRepository<CommentReport, Guid>
    {
        public Task<IEnumerable<CommentReport>> GetPendingReportsByCommentIdAsync(Guid commentId);
        public void UpdateRangeReports(List<CommentReport> reports);
    }
}
