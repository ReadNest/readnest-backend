using FluentValidation;
using ReadNest.Application.Models.Requests.Book;

namespace ReadNest.Application.Validators.Book
{
    public class CreateBookRequestValidator : AbstractValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
            _ = RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            _ = RuleFor(x => x.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author cannot exceed 100 characters.");

            _ = RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                //.Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
                .WithMessage("Image URL must be a valid URL.");

            _ = RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
            //.MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            _ = RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5).WithMessage("Rating must be between 0 and 5.");

            _ = RuleFor(x => x.ISBN)
                .NotEmpty().WithMessage("ISBN is required.");
            //.Matches(@"^\d{10}(\d{3})?$").WithMessage("ISBN must be 10 or 13 digits.");

            _ = RuleFor(x => x.Language)
                .NotEmpty().WithMessage("Language is required.")
                .MaximumLength(50).WithMessage("Language cannot exceed 50 characters.");

            _ = RuleFor(x => x.CategoryIds)
                .NotNull().WithMessage("CategoryIds must not be null.");
            //.Must(ids => ids != null && ids.Count > 0).WithMessage("At least one category must be selected.");
        }
    }
}
