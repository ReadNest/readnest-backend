using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Feature : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PackageFeature> PackageFeatures { get; set; }
    }
}
