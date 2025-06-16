using ReadNest.Shared.Common;

namespace ReadNest.Application.Models.Requests.Post
{
    public class FilterPostRequest : PagingRequest
    {
        public string? Keyword { get; set; }
        public Guid? BookId { get; set; }
        public string? SortBy { get; set; } // "views", "likes", "newest"
    }
}
