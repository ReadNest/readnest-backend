namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class CreateTradingPostRequestV2
    {
        public Guid UserId { get; set; }
        public string ExternalBookUrl { get; set; }
        public string Message { get; set; }
    }
}
