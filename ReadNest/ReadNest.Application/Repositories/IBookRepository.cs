using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.Repositories
{
    public interface IBookRepository : IGenericRepository<Book, Guid>
    {
        public Task<PagingResponse<GetBookSearchResponse>> FilterBooks(BookFilterRequest request);
        public Task<IEnumerable<Category>> GetCategoriesByBookIds(List<Guid> bookIds);
    }
}
