using ReadNest.Application.Models.Requests.FavoriteBook;
using ReadNest.Application.Models.Responses.FavoriteBook;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.FavoriteBook
{
    public interface IFavoriteBookUseCase
    {
        Task<ApiResponse<ToggleFavoriteBookResponse>> ToggleFavoriteAsync(ToggleFavoriteBookRequest request);
        Task<ApiResponse<List<Guid>>> GetFavoriteBookIdsByUserAsync(Guid userId);
    }
}
