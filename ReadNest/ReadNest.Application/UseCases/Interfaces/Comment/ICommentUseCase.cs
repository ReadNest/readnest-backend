using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Application.Models.Requests.Comment;
using ReadNest.Application.Models.Responses.Comment;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Comment
{
    public interface ICommentUseCase
    {
        Task<ApiResponse<GetCommentResponse>> CreateAsync(CreateCommentRequest request);
        //Get list of published comments by book id
        Task<ApiResponse<List<GetCommentResponse>>> GetPublishedCommentsByBookIdAsync(Guid bookId);
        // Like Comment
        Task<ApiResponse<string>> LikeCommentAsync(Guid commentId, Guid userId);
        Task<ApiResponse<string>> UpdateCommentAsync(UpdateCommentRequest request);
        Task<ApiResponse<string>> DeleteCommentAsync(Guid commentId);
        Task<ApiResponse<List<GetReportedCommentsResponse>>> GetAllPendingReportedCommentsAsync();
        Task<ApiResponse<List<GetCommentResponse>>> GetTop3RecentCommentsByUserIdAsync(Guid userId);
        Task<ApiResponse<List<GetCommentResponse>>> GetTop3MostLikedCommentsAsync();
    }
}
