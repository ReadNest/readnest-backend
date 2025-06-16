using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Post : BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string TitleNormalized { get; set; }
        public string Content { get; set; }
        public Guid BookId { get; set; }
        public Guid UserId { get; set; }
        public int Views { get; set; }
        public virtual Book Book { get; set; }
        public virtual User Creator { get; set; }
        public ICollection<User> Likes { get; set; } = new List<User>();
    }
}
