using ReadNest.Application.Models.Requests.CommentReport;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.CommentReport
{
    public interface ICommentReportUseCase
    {
        Task<ApiResponse<string>> CreateCommentReportAsync(CreateCommentReportRequest request);
        Task<ApiResponse<string>> ApproveCommentReportAndBan(Guid commentId);
        Task<ApiResponse<string>> RejectCommentReport(Guid commentId);
    }
}
