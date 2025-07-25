using ReadNest.Application.Models.Responses.Book;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Recommendation
{
    public interface IRecommendationUseCase
    {
        Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> RecommendBooksAsync(Guid userId, PagingRequest request);
        Task<ApiResponse<List<BookSuggestion>>> RecommendBooksByGeminiAsync(List<UserAnswer> answers);
    }
}
