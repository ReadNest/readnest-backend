using ReadNest.Application.Models.Responses.EventReward;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.EventReward
{
    public interface IEventRewardUseCase
    {
        Task<ApiResponse<IEnumerable<EventRewardResponse>>> GetRewardsByEventIdAsync(Guid eventId);
    }
}
