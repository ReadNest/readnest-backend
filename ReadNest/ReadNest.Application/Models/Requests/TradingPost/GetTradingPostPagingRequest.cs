using ReadNest.Shared.Common;

namespace ReadNest.Application.Models.Requests.TradingPost
{
    public class GetTradingPostPagingRequest : PagingRequest
    {
        public Guid UserId { get; set; }
    }
}
