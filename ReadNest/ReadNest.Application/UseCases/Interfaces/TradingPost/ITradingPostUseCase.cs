using ReadNest.Application.Models.Requests.TradingPost;
using ReadNest.Application.Models.Responses.TradingPost;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.TradingPost
{
    public interface ITradingPostUseCase
    {
        Task<ApiResponse<string>> CreateTradingPostAsync(CreateTradingPostRequest request);
        Task<ApiResponse<List<GetBookTradingPostV2Response>>> GetTopTradingPostsAsync(int? limit);
        Task<ApiResponse<PagingResponse<GetBookTradingPostResponse>>> GetTradingPostByUserIdAsync(GetTradingPostPagingRequest request);
        Task<ApiResponse<List<GetUserRequestResponse>>> GetUserRequestsByIdAsync(Guid tradingPostId);
        Task<ApiResponse<string>> DeleteTradingPostAsync(Guid tradingPostId);
        Task<ApiResponse<string>> UpdateStatusTradingRequestAsync(Guid tradingPostId, Guid tradingRequestId, UpdateStatusTradingRequest request);
        Task<ApiResponse<string>> CreateTradingRequestAsync(CreateTradingRequest request);
        Task<ApiResponse<string>> CreateTradingPostV2Async(CreateTradingPostRequestV2 request);
    }
}
