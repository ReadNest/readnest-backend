using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Recommendation;

namespace ReadNest.Application.UseCases.Implementations.Recommendation
{
    public class RecommendationUseCase : IRecommendationUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRedisUserTrackingService _redisUserTrackingService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteBookRepository"></param>
        /// <param name="bookRepository"></param>
        /// <param name="redisUserTrackingService"></param>
        public RecommendationUseCase(
            IBookRepository bookRepository,
            IRedisUserTrackingService redisUserTrackingService)
        {
            _bookRepository = bookRepository;
            _redisUserTrackingService = redisUserTrackingService;
        }

        public async Task<List<GetBookSearchResponse>> RecommendBooksAsync(Guid userId)
        {
            var recommendedBooks = new List<GetBookSearchResponse>();

            var booksFromFavorites = await _bookRepository.RecommendFromFavoritesBooksAsync(userId);
            recommendedBooks.AddRange(MapBooks(booksFromFavorites));

            if (recommendedBooks.Count < 5)
            {
                var topClickedBookIds = await _redisUserTrackingService.GetTopBookClicksAsync(userId, 3);
                if (topClickedBookIds.Any())
                {
                    var booksFromClicks = await _bookRepository.RecommendFromBookIdsAsync(topClickedBookIds);
                    var mapped = MapBooks(booksFromClicks);

                    // Bỏ trùng
                    recommendedBooks.AddRange(mapped
                        .Where(b => !recommendedBooks.Any(r => r.Id == b.Id)));
                }
            }

            if (recommendedBooks.Count < 5)
            {
                var topKeywords = await _redisUserTrackingService.GetTopKeywordsAsync(userId, 3);
                if (topKeywords.Any())
                {
                    var booksFromKeywords = await _bookRepository.RecommendFromKeywordsAsync(topKeywords);
                    var mapped = MapBooks(booksFromKeywords);

                    recommendedBooks.AddRange(mapped
                        .Where(b => !recommendedBooks.Any(r => r.Id == b.Id)));
                }
            }

            if (!recommendedBooks.Any())
            {
                var popularBooks = await _bookRepository.GetPopularBooksAsync(10);
                recommendedBooks.AddRange(MapBooks(popularBooks));
            }

            return recommendedBooks.ToList();
        }

        private List<GetBookSearchResponse> MapBooks(IEnumerable<Domain.Entities.Book> books)
        {
            return books.Select(x => new GetBookSearchResponse
            {
                Id = x.Id,
                Author = x.Author,
                AverageRating = x.AvarageRating,
                ImageUrl = x.ImageUrl,
                ShortDescription = x.Description,
                Title = x.Title
            }).ToList();
        }
    }
}
