using FluentValidation;
using ReadNest.Application.Models.Requests.Auth;

namespace ReadNest.Application.Validators.Auth
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            _ = RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Tên đăng nhập là bắt buộc.")
                .Length(3, 100).WithMessage("Tên đăng nhập phải có độ dài từ 3 đến 100 ký tự.");

            _ = RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Mật khẩu là bắt buộc.")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự.");
        }
    }
}
