using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class AffiliateLink : BaseEntity<Guid>
    {
        public string Link { get; set; }
        public string PartnerName { get; set; }
        public Guid BookId { get; set; }
        public virtual Book Book { get; set; }
    }
}
