using ReadNest.Application.Models.Requests.Post;
using ReadNest.Application.Models.Responses.Post;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Post
{
    public interface IPostUseCase
    {
        Task<ApiResponse<List<GetPostResponse>>> GetAllPostsAsync();
        Task<ApiResponse<GetPostResponse>> CreateAsync(CreatePostRequest request);
        Task<ApiResponse<GetPostResponse>> GetPostByIdAsync(Guid postId);
        Task<ApiResponse<List<GetPostResponse>>> GetPostsByUserIdAsync(Guid userId);
        Task<ApiResponse<List<GetPostResponse>>> GetPostsByBookIdAsync(Guid bookId);
        Task<ApiResponse<List<GetPostResponse>>> GetTopMostLikedPostsAsync(int count);
        Task<ApiResponse<List<GetPostResponse>>> GetTopMostViewedPostsAsync(int count);
        Task<ApiResponse<List<GetPostResponse>>> SearchByTitleAsync(string keyword);
        Task<ApiResponse<string>> LikePostAsync(Guid postId, Guid userId);
    }
}
