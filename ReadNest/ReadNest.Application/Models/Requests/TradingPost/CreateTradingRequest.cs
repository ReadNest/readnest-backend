namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class CreateTradingRequest
    {
        public Guid UserId { get; set; }
        public Guid TradingPostId { get; set; }
    }
}
