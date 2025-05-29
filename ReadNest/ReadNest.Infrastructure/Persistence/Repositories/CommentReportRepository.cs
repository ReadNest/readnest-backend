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
    public class CommentReportRepository(AppDbContext context) : GenericRepository<CommentReport, Guid>(context), ICommentReportRepository
    {
        public async Task<IEnumerable<CommentReport>> GetPendingReportsByCommentIdAsync(Guid commentId)
        {
            var reports = await _context.CommentReports
                .Where(report => report.CommentId == commentId && report.Status == "Pending")
                .ToListAsync();
            return reports;
        }

        public void UpdateRangeReports(List<CommentReport> reports)
        {
             _context.CommentReports.UpdateRange(reports);
        }
    }
}
