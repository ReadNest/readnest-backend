using FluentValidation;
using ReadNest.Application.Models.Requests.Post;

namespace ReadNest.Application.Validators.Post
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            _ = RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            _ = RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(5000).WithMessage("Content must not exceed 5000 characters.");

            _ = RuleFor(x => x.BookId)
                .NotEmpty().WithMessage("Book ID is required.");

            _ = RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User ID is required.");
        }
    }
}
