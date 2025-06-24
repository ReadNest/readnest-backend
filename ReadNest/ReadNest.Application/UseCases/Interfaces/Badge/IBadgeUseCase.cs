using ReadNest.Application.Models.Requests.Badge;
using ReadNest.Application.Models.Responses.Badge;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Badge
{
    public interface IBadgeUseCase
    {
        public Task<ApiResponse<List<GetBadgeResponse>>> GetBadgesAsync();
        public Task<ApiResponse<CreateBadgeResponse>> CreateBadgeAsync(CreateBadgeRequest request);
        public Task<ApiResponse<string>> SoftDeleteBadgeByCodeAsync(string code);
        public Task<ApiResponse<string>> UpdateBadgeAsync(UpdateBadgeRequest request);
    }
}
