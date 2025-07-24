using ReadNest.Application.Models.Responses.Book;

namespace ReadNest.Application.UseCases.Interfaces.Recommendation
{
    public interface IRecommendationUseCase
    {
        Task<List<GetBookSearchResponse>> RecommendBooksAsync(Guid userId);
    }
}
