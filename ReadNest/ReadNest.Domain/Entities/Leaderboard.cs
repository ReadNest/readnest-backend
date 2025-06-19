using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Leaderboard : BaseEntity<Guid>
    {
        public int TotalPosts { get; set; }
        public int TotalLikes { get; set; }
        public int TotalViews { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public Guid UserId { get; set; }
        public virtual User User { get; set; }
        public Guid EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}
