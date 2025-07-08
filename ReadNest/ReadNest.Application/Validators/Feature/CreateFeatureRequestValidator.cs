using FluentValidation;
using ReadNest.Application.Models.Requests.Feature;

namespace ReadNest.Application.Validators.Feature
{
    public class CreateFeatureRequestValidator : AbstractValidator<CreateFeatureRequest>
    {
        public CreateFeatureRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Tên tính năng không được để trống.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Hãy nhập mô tả cho tính năng.");
        }
    }
}
