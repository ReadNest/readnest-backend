namespace ReadNest.Application.Models.Responses.TradingPost
{
    public class GetUserRequestResponse
    {
        public Guid UserId { get; set; }
        public Guid TradingRequestId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string AvatarUrl { get; set; }
        public string Status { get; set; }
    }
}
