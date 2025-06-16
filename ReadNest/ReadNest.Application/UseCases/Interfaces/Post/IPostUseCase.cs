using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Post
{
    public interface IPostUseCase
    {
        Task<ApiResponse<PagingResponse<GetPostResponse>>> GetAllPostsAsync(PagingRequest request);
        Task<ApiResponse<GetPostResponse>> CreateAsync(CreatePostRequest request);
        Task<ApiResponse<GetPostResponse>> GetPostByIdAsync(Guid postId);
        Task<ApiResponse<PagingResponse<GetPostResponse>>> GetPostsByUserIdAsync(Guid userId, PagingRequest request);
        Task<ApiResponse<List<GetPostResponse>>> GetPostsByBookIdAsync(Guid bookId);
        Task<ApiResponse<List<GetPostResponse>>> GetTopMostLikedPostsAsync(int count);
        Task<ApiResponse<List<GetPostResponse>>> GetTopMostViewedPostsAsync(int count);
        Task<ApiResponse<List<GetPostResponse>>> SearchByTitleAsync(string keyword);
        Task<ApiResponse<string>> LikePostAsync(Guid postId, Guid userId);
        Task<ApiResponse<GetPostResponse>> UpdateAsync(UpdatePostRequest request);
        Task<ApiResponse<string>> DeleteAsync(Guid postId);
        Task<ApiResponse<string>> IncreasePostViewsAsync(Guid postId);
    }
}
