using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class BookImage : BaseEntity<Guid>
    {
        public Guid BookId { get; set; }
        public string ImageUrl { get; set; }
        public int Order { get; set; }
        public Book Book { get; set; }
    }
}
