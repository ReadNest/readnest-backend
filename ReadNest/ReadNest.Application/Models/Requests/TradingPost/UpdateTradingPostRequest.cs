namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class UpdateTradingPostRequest
    {
        public string Title { get; set; }
        public string Condition { get; set; }
        public string ShortDescription { get; set; }
        public string MessageToRequester { get; set; }
        public List<UpdateTradingPostImageRequest> Images { get; set; } = new();
    }
}
