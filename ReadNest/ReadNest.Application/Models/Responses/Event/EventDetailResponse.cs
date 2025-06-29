
using ReadNest.Application.Models.Responses.EventReward;
using ReadNest.Application.Models.Responses.Leaderboard;

namespace ReadNest.Application.Models.Responses.Event
{
    public class EventDetailResponse
    {
        public EventResponse Event { get; set; }
        public List<LeaderboardResponse> Leaderboards { get; set; }
        public List<EventRewardResponse> Rewards { get; set; }
    }
}
