using FluentValidation;
using ReadNest.Application.Models.Requests.Auth;

namespace ReadNest.Application.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            _ = RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.");

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");
        }
    }
}
