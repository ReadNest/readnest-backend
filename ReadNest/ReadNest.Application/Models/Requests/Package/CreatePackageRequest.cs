namespace ReadNest.Application.Models.Requests.Package
{
    public class CreatePackageRequest
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public string Features { get; set; } // Description or metadata for features
        public List<CreatePackageFeatureRequest> PackageFeatures { get; set; }
    }
}
