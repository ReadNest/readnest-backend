using FluentValidation;
using ReadNest.Application.Models.Requests.AffiliateLink;

namespace ReadNest.Application.Validators.AffiliateLink
{
    public class AffiliateLinkRequestValidator : AbstractValidator<AffiliateLinkRequest>
    {
        public AffiliateLinkRequestValidator()
        {
            _ = RuleFor(x => x.PartnerName)
                .NotEmpty().WithMessage("Partner name is required.")
                .MaximumLength(100).WithMessage("Partner name must be at most 100 characters.");

            _ = RuleFor(x => x.AffiliateLink)
                .NotEmpty().WithMessage("Affiliate link is required.")
                //.Must(link => Uri.TryCreate(link, UriKind.Absolute, out _))
                .WithMessage("Affiliate link must be a valid URL.");
        }
    }
}
