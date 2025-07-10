namespace ReadNest.Application.Models.Requests.Payment
{
    public class CreatePaymentLinkRequest
    {
        public Guid PackageId { get; set; }
        public Guid UserId { get; set; }
    }
}
