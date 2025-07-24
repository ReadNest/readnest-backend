using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.TradingPost;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Book
{
    public interface IBookUseCase
    {
        Task<ApiResponse<GetBookResponse>> CreateBookAsync(CreateBookRequest request);
        Task<ApiResponse<string>> UpdateBookAsync(Guid bookId, UpdateBookRequest request);
        Task<ApiResponse<PagingResponse<GetBookResponse>>> GetAllAsync(PagingRequest request);
        Task<ApiResponse<GetBookResponse>> GetByIdAsync(Guid bookId, string token);
        Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> SearchBooksAsync(PagingRequest paging, string? keyword, string token);
        Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> FilterBooksAsync(BookFilterRequest request, string token);
        Task<ApiResponse<string>> DeleteBookAsync(Guid bookId);
        Task<ApiResponse<List<GetBookTradingPostResponse>>> GetBookTradingPostAsync();
    }
}
