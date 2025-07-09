using FluentValidation;
using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Requests.Package;
using ReadNest.Application.Models.Responses.Package;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Package;
using ReadNest.Application.Validators.Package;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Package
{
    public class PackageUseCase : IPackageUseCase
    {
        private readonly IPackageRepository _packageRepository;
        private readonly CreatePackageRequestValidator _createValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packageRepository"></param>
        /// <param name="createValidator"></param>
        public PackageUseCase(IPackageRepository packageRepository, CreatePackageRequestValidator createValidator)
        {
            _packageRepository = packageRepository;
            _createValidator = createValidator;
        }

        public async Task<ApiResponse<string>> CreatePackageAsync(CreatePackageRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var package = new Domain.Entities.Package
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                DurationMonths = request.DurationMonths,
                Features = request.Features,
            };

            var packageFeatures = request.PackageFeatures.Select(x => new Domain.Entities.PackageFeature
            {
                PackageId = package.Id,
                FeatureId = x.FeatureId,
            }).ToList();

            package.PackageFeatures = packageFeatures;

            _ = await _packageRepository.AddAsync(package);
            await _packageRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(package.Id.ToString());
        }

        public async Task<ApiResponse<string>> DeletePackageAsync(Guid id)
        {
            var package = await _packageRepository.GetByIdAsync(id);
            if (package == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0005);
            }

            await _packageRepository.SoftDeleteAsync(package);
            await _packageRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(id.ToString());
        }

        public async Task<ApiResponse<PagingResponse<GetPackageResponse>>> GetPackgesWithPagingAsync(PagingRequest request)
        {
            var response = await _packageRepository.GetPagedAsync(
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                include: query => query.Include(x => x.PackageFeatures));

            var responseConverted = new PagingResponse<GetPackageResponse>
            {
                PageSize = response.PageSize,
                PageIndex = response.PageIndex,
                TotalItems = response.TotalItems,
                Items = response.Items.Select(x => new GetPackageResponse
                {
                    DurationMonths = x.DurationMonths,
                    Features = x.Features,
                    Name = x.Name,
                    Price = x.Price,
                    FeatureNames = x.PackageFeatures.Select(x => x.Feature.Name).ToList()
                })
            };

            return ApiResponse<PagingResponse<GetPackageResponse>>.Ok(responseConverted);
        }
    }
}
