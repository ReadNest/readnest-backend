namespace ReadNest.Application.Models.Requests.AffiliateLink
{
    public class UpdateAffiliateLinkRequest
    {
        public List<UpdateAffiliateLink> UpdateAffiliateLinks { get; set; }
    }

    public record UpdateAffiliateLink(Guid? id, string partnerName, string affilateLink);
}
