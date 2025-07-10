using ReadNest.Domain.Base;

namespace ReadNest.Domain.Entities
{
    public class Package : BaseEntity<Guid>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public string Features { get; set; } // Description or metadata for features
        public ICollection<UserSubscription> UserSubscriptions { get; set; }
        public ICollection<PackageFeature> PackageFeatures { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}
