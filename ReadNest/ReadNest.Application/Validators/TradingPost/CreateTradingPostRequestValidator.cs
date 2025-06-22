using FluentValidation;
using ReadNest.Application.Models.Requests.TradingPost;

namespace ReadNest.Application.Validators.TradingPost
{
    public class CreateTradingPostRequestValidator : AbstractValidator<CreateTradingPostRequest>
    {
        public CreateTradingPostRequestValidator()
        {
            _ = RuleFor(x => x.UserId)
                .NotNull().WithMessage("UserId is required.")
                .NotEmpty().WithMessage("UserId cannot be empty.");

            _ = RuleFor(x => x.BookId)
                .NotNull().WithMessage("BookId is required.")
                .NotEmpty().WithMessage("BookId cannot be empty.");

            _ = RuleFor(x => x.Condition)
                .NotNull().WithMessage("Condition is required.")
                .NotEmpty().WithMessage("Condition cannot be empty.");

            _ = RuleFor(x => x.ShortDescription)
                .NotNull().WithMessage("ShortDescription is required.")
                .NotEmpty().WithMessage("ShortDescription cannot be empty.");

            _ = RuleFor(x => x.MessageToRequester)
                .NotNull().WithMessage("MessageToRequester is required.")
                .NotEmpty().WithMessage("MessageToRequester cannot be empty.");

            _ = RuleFor(x => x.Title)
                .NotNull().WithMessage("Title is required.")
                .NotEmpty().WithMessage("Title cannot be empty.");

            _ = RuleFor(x => x.Images)
                .NotNull().WithMessage("Images are required.")
                .Must(images => images!.Count > 0).WithMessage("Images list cannot be empty.");

            _ = RuleForEach(x => x.Images!).SetValidator(new CreateTradingPostImageRequestValidator());
        }
    }
}
