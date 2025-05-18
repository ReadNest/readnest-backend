namespace ReadNest.Application.Models.Requests.AffiliateLink
{
    public class CreateAffiliateLinkRequest
    {
        public List<AffiliateLinkRequest> AffiliateLinkRequests { get; set; }
    }

    public class AffiliateLinkRequest
    {
        public string PartnerName { get; set; }
        public string AffiliateLink { get; set; }
    }
}
