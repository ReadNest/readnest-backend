using FluentValidation;
using ReadNest.Application.Models.Requests.AffiliateLink;

namespace ReadNest.Application.Validators.AffiliateLink
{
    public class CreateAffiliateLinkRequestValidator : AbstractValidator<CreateAffiliateLinkRequest>
    {
        public CreateAffiliateLinkRequestValidator()
        {
            _ = RuleFor(x => x.AffiliateLinkRequests)
                .NotNull().WithMessage("Affiliate link requests list cannot be null.")
                .Must(x => x.Count > 0).WithMessage("Affiliate link request must contain at least one item.");

            _ = RuleForEach(x => x.AffiliateLinkRequests)
                .SetValidator(new AffiliateLinkRequestValidator());
        }
    }
}
