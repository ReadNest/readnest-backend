using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Recommendation;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Recommendation
{
    public class RecommendationUseCase : IRecommendationUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IRedisUserTrackingService _redisUserTrackingService;
        private readonly IGeminiService _geminiService;
        private readonly IBookCoverService _bookCoverService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteBookRepository"></param>
        /// <param name="bookRepository"></param>
        /// <param name="redisUserTrackingService"></param>
        /// <param name="bookCoverService"></param>
        /// <param name="geminiService"></param>
        public RecommendationUseCase(
            IBookRepository bookRepository,
            IRedisUserTrackingService redisUserTrackingService,
            IGeminiService geminiService,
            IBookCoverService bookCoverService)
        {
            _bookRepository = bookRepository;
            _redisUserTrackingService = redisUserTrackingService;
            _geminiService = geminiService;
            _bookCoverService = bookCoverService;
        }

        public async Task<ApiResponse<PagingResponse<GetBookSearchResponse>>> RecommendBooksAsync(Guid userId, PagingRequest request)
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
                }
            }

            if (recommendedBooks.Count < 5)
            {
                var topKeywords = await _redisUserTrackingService.GetTopKeywordsAsync(userId, 3);
                if (topKeywords.Any())
                {
                    var booksFromKeywords = await _bookRepository.RecommendFromKeywordsAsync(topKeywords);
                    var mapped = MapBooks(booksFromKeywords);
                }
            }

            if (!recommendedBooks.Any())
            {
                var popularBooks = await _bookRepository.GetPopularBooksAsync(10);
                recommendedBooks.AddRange(MapBooks(popularBooks));
            }

            recommendedBooks = recommendedBooks.DistinctBy(b => b.Id).ToList();

            var response = new PagingResponse<GetBookSearchResponse>
            {
                TotalItems = recommendedBooks.Count,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = recommendedBooks.Skip((request.PageIndex - 1) * request.PageSize)
                                        .Take(request.PageSize),
            };

            return ApiResponse<PagingResponse<GetBookSearchResponse>>.Ok(response);
        }

        public async Task<ApiResponse<List<BookSuggestion>>> RecommendBooksByGeminiAsync(List<UserAnswer> answers)
        {
            var books = await _geminiService.GetRecommendationsAsync(answers);

            foreach (var book in books)
            {
                var cover = await _bookCoverService.GetBookInfoAsync(book.Title, book.Author);
                if (cover != null)
                {
                    book.Image = cover.Thumbnail;
                    book.InfoLink = cover.InfoLink;
                }
                else
                {
                    book.Image = "https://via.placeholder.com/150";
                }
            }

            return ApiResponse<List<BookSuggestion>>.Ok(books);
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
