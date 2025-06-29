namespace ReadNest.Application.Models.Responses.TradingPost
{
    public class GetBookTradingPostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
        public string Condition { get; set; }
        public List<Guid> TradingRequestIds { get; set; }
    }
}
