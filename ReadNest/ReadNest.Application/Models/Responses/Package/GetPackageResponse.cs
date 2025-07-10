namespace ReadNest.Application.Models.Responses.Package
{
    public class GetPackageResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationMonths { get; set; }
        public string Features { get; set; }
        public List<string> FeatureNames { get; set; }
    }
}
