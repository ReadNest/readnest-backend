using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class EventReward : BaseEntity<Guid>
    {
        public string ConditionType { get; set; } // e.g., "RankTop", "ScoreAbove"
        public int Threshold { get; set; }
        public Guid BadgeId { get; set; }
        public Guid EventId { get; set; }
        public virtual Badge Badge { get; set; }
        public virtual Event Event { get; set; }
    }
}
