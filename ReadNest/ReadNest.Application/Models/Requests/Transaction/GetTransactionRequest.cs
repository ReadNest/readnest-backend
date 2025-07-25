using ReadNest.Shared.Common;

namespace ReadNest.Application.Models.Requests.Transaction
{
    public class GetTransactionRequest : PagingRequest
    {
        public string? Status { get; set; } = null;
    }
}
