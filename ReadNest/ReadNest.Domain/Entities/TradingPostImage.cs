using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class TradingPostImage : BaseEntity<Guid>
    {
        public Guid Id { get; set; }
        public Guid TradingPostId { get; set; }
        public string ImageUrl { get; set; } = null!;
        public int Order { get; set; }
        public TradingPost TradingPost { get; set; } = null!;
    }
}
