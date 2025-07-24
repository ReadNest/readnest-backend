using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.Repositories
{
    public interface IBookRepository : IGenericRepository<Book, Guid>
    {
        Task<PagingResponse<GetBookSearchResponse>> FilterBooks(BookFilterRequest request);
        Task<IEnumerable<Category>> GetCategoriesByBookIds(List<Guid> bookIds);
        Task<List<Book>> RecommendFromFavoritesBooksAsync(Guid userId);
        Task<List<Book>> RecommendFromBookIdsAsync(List<Guid> bookIds);
        Task<List<Book>> GetPopularBooksAsync(int limit = 10);
        Task<List<Book>> RecommendFromKeywordsAsync(List<string> keywords);
    }
}
