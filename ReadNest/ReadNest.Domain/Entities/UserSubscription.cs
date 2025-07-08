using ReadNest.Domain.Base;
using ReadNest.Shared.Enums;

namespace ReadNest.Domain.Entities
{
    public class UserSubscription : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public Guid PackageId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = StatusEnum.Active.ToString();
        public User User { get; set; }
        public Package Package { get; set; }
    }
}
