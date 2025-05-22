using ReadNest.Application.Models.Requests.FavoriteBook;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.FavoriteBook;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.FavoriteBook
{
    public interface IFavoriteBookUseCase
    {
        Task<ApiResponse<ToggleFavoriteBookResponse>> ToggleFavoriteAsync(ToggleFavoriteBookRequest request);
        Task<ApiResponse<PagingResponse<GetBookResponse>>> GetFavoriteBooksPagedByUserAsync(Guid userId, PagingRequest request);
    }
}
