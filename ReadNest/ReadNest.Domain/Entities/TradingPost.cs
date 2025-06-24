using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class TradingPost : BaseEntity<Guid>
    {
        public Guid OwnerId { get; set; }
        public User Owner { get; set; }
        public Guid OfferedBookId { get; set; }
        public Book OfferedBook { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string Condition { get; set; }
        public string ShortDesc { get; set; }
        public string? ExternalBookUrl { get; set; }
        public string? Message { get; set; }
        public string MessageToRequester { get; set; }
        public ICollection<TradingRequest> TradingRequests { get; set; }
        public ICollection<TradingPostImage> Images { get; set; }
    }
}
