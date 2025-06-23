using ReadNest.Application.Models.Responses.Badge;

namespace ReadNest.Application.Models.Responses.EventReward
{
    public class EventRewardResponse
    {
        public Guid Id { get; set; }
        public string ConditionType { get; set; } = null!;
        public int Threshold { get; set; }
        public Guid BadgeId { get; set; }
        public Guid EventId { get; set; }
        public virtual GetBadgeResponse Badge { get; set; } = null!;
    }
}
