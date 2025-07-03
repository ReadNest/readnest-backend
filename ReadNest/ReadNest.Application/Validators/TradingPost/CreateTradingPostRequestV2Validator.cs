using FluentValidation;
using ReadNest.Application.Models.Requests.TradingPost;

namespace ReadNest.Application.Validators.TradingPost
{
    public class CreateTradingPostRequestV2Validator : AbstractValidator<CreateTradingPostRequestV2>
    {
        public CreateTradingPostRequestV2Validator()
        {
            _ = RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId không được để trống.");
            _ = RuleFor(x => x.ExternalBookUrl).NotEmpty().WithMessage("Link sách không được để trống.");
            _ = RuleFor(x => x.Message).NotEmpty().WithMessage("Lý do tạo sách không được để trống.");
        }
    }
}
