using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Badge : BaseEntity<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<UserBadge> UserBadges { get; set; }
        public ICollection<EventReward> EventRewards { get; set; }
    }
}
