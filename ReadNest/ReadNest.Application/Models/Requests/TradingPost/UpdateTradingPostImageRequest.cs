namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class UpdateTradingPostImageRequest
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
    }
}
