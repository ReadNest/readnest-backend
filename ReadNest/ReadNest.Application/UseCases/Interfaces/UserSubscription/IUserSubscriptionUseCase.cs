using ReadNest.Application.Models.Responses.UserSubscription;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.UserSubscription
{
    public interface IUserSubscriptionUseCase
    {
        Task<ApiResponse<GetUserSubscriptionResponse>> GetUserSubscriptionByUserIdAsync(Guid userId);       
    }
}
