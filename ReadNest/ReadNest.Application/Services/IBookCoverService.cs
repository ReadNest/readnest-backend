using ReadNest.Shared.Common;

namespace ReadNest.Application.Services
{
    public interface IBookCoverService
    {
        Task<BookSearchResult> GetBookInfoAsync(string title, string author);
    }
}
