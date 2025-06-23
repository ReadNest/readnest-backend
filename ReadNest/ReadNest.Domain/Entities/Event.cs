using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Event : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Type { get; set; } // e.g., "Weekly", "Monthly", "Special", etc.
        public string Status { get; set; } // e.g., "Upcoming", "Ongoing", "Ended"
        public ICollection<Leaderboard> Leaderboards { get; set; } 
        public ICollection<EventReward> Rewards { get; set; } 
    }
}
