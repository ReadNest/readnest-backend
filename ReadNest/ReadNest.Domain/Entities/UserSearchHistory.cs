using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class UserSearchHistory : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Keyword { get; set; }
        public User User { get; set; }
    }
}
