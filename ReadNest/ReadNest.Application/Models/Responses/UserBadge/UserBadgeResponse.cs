namespace ReadNest.Application.Models.Responses.UserBadge
{
    public class UserBadgeResponse
    {
        public Guid UserBadgeId { get; set; }
        public Guid UserId { get; set; }
        public Guid BadgeId { get; set; }
        public string BadgeCode { get; set; }
        public string BadgeName { get; set; }
        public string BadgeDescription { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
