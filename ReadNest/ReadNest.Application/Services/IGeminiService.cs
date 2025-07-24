using ReadNest.Shared.Common;

namespace ReadNest.Application.Services
{
    public interface IGeminiService
    {
        Task<List<BookSuggestion>> GetRecommendationsAsync(List<UserAnswer> answers);
    }
}
