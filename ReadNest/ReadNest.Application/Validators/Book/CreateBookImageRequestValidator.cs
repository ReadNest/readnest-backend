using FluentValidation;
using ReadNest.Application.Models.Requests.Book;

namespace ReadNest.Application.Validators.Book
{
    public class CreateBookImageRequestValidator : AbstractValidator<CreateBookImageRequest>
    {
        public CreateBookImageRequestValidator()
        {
            _ = RuleFor(x => x.ImageUrl).NotEmpty();
            _ = RuleFor(x => x.Order).GreaterThanOrEqualTo(0);
        }
    }
}
