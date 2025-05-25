using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Book
{
    public interface IBookUseCase
    {
        Task<ApiResponse<GetBookResponse>> CreateBookAsync(CreateBookRequest request);
        Task<ApiResponse<PagingResponse<GetBookResponse>>> GetAllAsync(PagingRequest request);
        Task<ApiResponse<GetBookResponse>> GetByIdAsync(Guid bookId);
        Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> SearchBooksAsync(PagingRequest paging, string? keyword);
    }
}
