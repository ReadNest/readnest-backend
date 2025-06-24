using ReadNest.Application.Models.Responses.Badge;
using ReadNest.Application.Models.Responses.EventReward;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.EventReward;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.EventReward
{
    public class EventRewardUseCase : IEventRewardUseCase
    {
        private readonly IEventRewardRepository _eventRewardRepository;

        public EventRewardUseCase(IEventRewardRepository eventRewardRepository)
        {
            _eventRewardRepository = eventRewardRepository;
        }

        public async Task<ApiResponse<IEnumerable<EventRewardResponse>>> GetRewardsByEventIdAsync(Guid eventId)
        {
            var rewards = await _eventRewardRepository.GetRewardsByEventIdAsync(eventId);
            var result = rewards.Select(r => new EventRewardResponse
            {
                Id = r.Id,
                ConditionType = r.ConditionType,
                Threshold = r.Threshold,
                BadgeId = r.BadgeId,
                EventId = r.EventId,
                Badge = new GetBadgeResponse
                {
                    Id = r.Badge.Id,
                    Name = r.Badge.Name,
                    Description = r.Badge.Description,
                    Code = r.Badge.Code
                }
            });

            return ApiResponse<IEnumerable<EventRewardResponse>>.Ok(result);
        }
    }
}
