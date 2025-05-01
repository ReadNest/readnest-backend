using FluentValidation;
using ReadNest.Application.Models.Requests.Auth;
using ReadNest.Application.Repositories;

namespace ReadNest.Application.Validators.Auth
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator(IUserRepository userRepository)
        {
            _ = RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(3, 100).WithMessage("Username must be between 3 and 100 characters.")
                .MustAsync(async (userName, cancellation) => !await userRepository.ExistsByUserNameAsync(userName))
                .WithMessage("UserName already is taken.");

            _ = RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Length(0, 255).WithMessage("Email must be less than 255 characters.")
                .MustAsync(async (email, cancellation) => !await userRepository.ExistsByEmailAsync(email) == true)
                .WithMessage("Email already is taken.");

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters.");

            _ = RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .Equal(x => x.Password).WithMessage("Confirm Password must match Password.");

            _ = RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .Length(0, 500).WithMessage("Address must be less than 500 characters.");

            _ = RuleFor(x => x.DateOfBirth)
                .NotEmpty().WithMessage("Date of Birth is required.")
                .LessThan(DateTime.UtcNow).WithMessage("Date of Birth must be in the past.");
        }
    }
}
