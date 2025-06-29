using FluentValidation;
using ReadNest.Application.Models.Requests.TradingPost;

namespace ReadNest.Application.Validators.TradingPost
{
    public class CreateTradingPostImageRequestValidator : AbstractValidator<CreateTradingPostImageRequest>
    {
        public CreateTradingPostImageRequestValidator()
        {
            _ = RuleFor(x => x.Order)
                .NotNull().WithMessage("Order is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Order must be >= 0.");

            _ = RuleFor(x => x.ImageUrl)
                .NotNull().WithMessage("ImageUrl is required.")
                .NotEmpty().WithMessage("ImageUrl cannot be empty.");
        }
    }
}
