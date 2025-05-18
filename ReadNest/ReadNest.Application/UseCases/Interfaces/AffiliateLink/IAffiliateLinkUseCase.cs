using ReadNest.Application.Models.Requests.AffiliateLink;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.AffiliateLink
{
    public interface IAffiliateLinkUseCase
    {
        Task<ApiResponse<string>> CreateAsync(Guid bookId, CreateAffiliateLinkRequest request);
    }
}
