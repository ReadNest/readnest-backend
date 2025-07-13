using ReadNest.Shared.Enums;

namespace ReadNest.Application.Models.Responses.UserSubscription
{
    public record GetUserSubscriptionResponse(DateTime StartDate, DateTime? EndDate, string Status, string PackageName);
}
