namespace ReadNest.Application.Models.Responses.TradingPost
{
    public class GetBookTradingPostV2Response
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string OwnerName { get; set; }
        public string UserName { get; set; }
        public string ImageUrl { get; set; }
        public string Condition { get; set; }
        public string ShortDesc { get; set; }
        public string MessageToRequester { get; set; }
        public int NumberOfTradingRequests { get; set; }
        public List<GetTradingPostImageResponse> Images { get; set; }
    }
}
