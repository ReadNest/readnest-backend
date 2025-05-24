using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Comment : BaseEntity<Guid>
    {
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public string Status { get; set; }
        public virtual Book Book { get; set; }
        public virtual User Creator { get; set; }
        public ICollection<User> Likes { get; set; }
    }
}
