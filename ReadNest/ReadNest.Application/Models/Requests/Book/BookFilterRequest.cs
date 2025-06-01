using ReadNest.Shared.Common;

namespace ReadNest.Application.Models.Requests.Book
{
    public class BookFilterRequest : PagingRequest
    {
        public List<Guid> CategoryIds { get; set; } = new();
        public List<string> LanguageIds { get; set; } = new();
        public string? Keyword { get; set; }
    }
}
