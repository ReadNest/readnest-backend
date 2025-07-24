using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Recommendation;

namespace ReadNest.Application.UseCases.Implementations.Recommendation
{
    public class RecommendationUseCase : IRecommendationUseCase
    {
        private readonly IBookRepository _bookRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteBookRepository"></param>
        /// <param name="bookRepository"></param>
        public RecommendationUseCase(
            IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<List<GetBookSearchResponse>> RecommendBooksAsync(Guid userId)
        {
            var booksInSameCategory = await _bookRepository.RecommendFromFavoritesBooksAsync(userId);
            return booksInSameCategory.Select(x => new GetBookSearchResponse
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
