namespace ReadNest.Application.Models.Responses.TradingPost
{
    public class GetTradingPostImageResponse
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
