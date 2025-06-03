using ReadNest.Application.Models.Requests.CommentReport;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.CommentReport;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.CommentReport
{
    public class CommentReportUseCase : ICommentReportUseCase
    {
        private readonly ICommentReportRepository _commentReportRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentReportUseCase(ICommentReportRepository commentReportRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _commentReportRepository = commentReportRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<ApiResponse<string>> ApproveCommentReportAndBan(Guid commentId)
        {
            var comment = await _commentRepository.GetPublishCommentByIdAsync(commentId);
            if (comment == null)
            {
                return ApiResponse<string>.Fail("Invalid Comment");
            }
            comment.Status = "Banned";
            await _commentRepository.SaveChangesAsync();
            var reports = await _commentReportRepository.GetPendingReportsByCommentIdAsync(commentId);
            if (reports == null || !reports.Any())
            {
                return ApiResponse<string>.Fail("No pending reports found for this comment");
            }
            foreach (var report in reports)
            {
                report.Status = "Violated";
            }
            _commentReportRepository.UpdateRangeReports((List<Domain.Entities.CommentReport>)reports);
            await _commentReportRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Approve comment report and ban comment successfully");
        }

        public async Task<ApiResponse<string>> CreateCommentReportAsync(CreateCommentReportRequest request)
        {
            var reporter = await _userRepository.GetByIdAsync(request.ReporterId);
            if (reporter == null)
            {
                return ApiResponse<string>.Fail("Invalid Reporter");
            }

            var comment = await _commentRepository.GetPublishCommentByIdAsync(request.CommentId);
            if (comment == null)
            {
                return ApiResponse<string>.Fail("Invalid Comment");
            }

            if (string.IsNullOrWhiteSpace(request.Reason))
            {
                return ApiResponse<string>.Fail("Reason cannot be empty");
            }
            if (request.Reason.Length > 255)
            {
                return ApiResponse<string>.Fail("Reason is too long");
            }

            var report = new Domain.Entities.CommentReport
            {
                CommentId = request.CommentId,
                ReporterId = request.ReporterId,
                Reason = request.Reason,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            var result = await _commentReportRepository.AddAsync(report);
            if (result == null)
            {
                return ApiResponse<string>.Fail("Failed to create comment report");
            }
            await _commentReportRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Report comment successfully");
        }

        public async Task<ApiResponse<string>> RejectCommentReport(Guid commentId)
        {
            var reports = await _commentReportRepository.GetPendingReportsByCommentIdAsync(commentId);
            if (reports == null || !reports.Any())
            {
                return ApiResponse<string>.Fail("No pending reports found for this comment");
            }
            foreach (var report in reports)
            {
                report.Status = "NotViolated";
            }
            _commentReportRepository.UpdateRangeReports((List<Domain.Entities.CommentReport>)reports);
            await _commentReportRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Reject comment report successfully");
        }
    }
}
