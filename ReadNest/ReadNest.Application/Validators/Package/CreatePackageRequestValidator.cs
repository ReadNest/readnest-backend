using FluentValidation;
using ReadNest.Application.Models.Requests.Package;
using ReadNest.Application.Repositories;

namespace ReadNest.Application.Validators.Package
{
    public class CreatePackageRequestValidator : AbstractValidator<CreatePackageRequest>
    {
        private readonly IFeatureRepository _featureRepository;

        public CreatePackageRequestValidator(IFeatureRepository featureRepository)
        {
            _featureRepository = featureRepository;

            _ = RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên gói không được để trống.");

            _ = RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Giá phải lớn hơn 0.");

            _ = RuleFor(x => x.DurationMonths)
                .GreaterThan(0).WithMessage("Thời gian gói không hợp lệ.");

            _ = RuleFor(x => x.Features)
                .NotEmpty().WithMessage("Mô tả tính năng không được để trống.");

            RuleFor(x => x.PackageFeatures)
                .NotNull().WithMessage("Danh sách tính năng không được để trống.")
                .MustAsync(async (features, cancellation) => await AreFeatureIdsValid(features))
                .WithMessage("Một hoặc nhiều ID tính năng không hợp lệ.");
        }

        private async Task<bool> AreFeatureIdsValid(List<CreatePackageFeatureRequest> features)
        {
            if (features == null || !features.Any()) return true;

            var featureIds = features.Select(f => f.FeatureId).ToList();
            var invalidFeatureIds = await _featureRepository.GetInvalidFeatureIdsAsync(featureIds);

            return !invalidFeatureIds.Any();
        }
    }

}
