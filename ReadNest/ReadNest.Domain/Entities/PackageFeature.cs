using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class PackageFeature : BaseEntity<Guid>
    {
        public Guid PackageId { get; set; }
        public Guid FeatureId { get; set; }
        public Package Package { get; set; }
        public Feature Feature { get; set; }
    }
}
