namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class CreateTradingPostRequest
    {
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public string? Condition { get; set; }
        public string? ShortDescription { get; set; }
        public string? ExternalBookUrl { get; set; }
        public string? Message { get; set; }
        public string? MessageToRequester { get; set; }
        public List<CreateTradingPostImageRequest> Images { get; set; } = new();
        public string? Title { get; set; }
    }
}
