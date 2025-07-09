using FluentValidation;
using ReadNest.Application.Models.Requests.Feature;
using ReadNest.Application.Models.Responses.Feature;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Feature;
using ReadNest.Application.Validators.Feature;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Feature
{
    public class FeatureUseCase : IFeatureUseCase
    {
        private readonly CreateFeatureRequestValidator _createValidator;
        private readonly IFeatureRepository _featureRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="createValidator"></param>
        /// <param name="featureRepository"></param>
        public FeatureUseCase(CreateFeatureRequestValidator createValidator, IFeatureRepository featureRepository)
        {
            _createValidator = createValidator;
            _featureRepository = featureRepository;
        }

        public async Task<ApiResponse<string>> CreateFeatureAsync(CreateFeatureRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var feature = new Domain.Entities.Feature
            {
                Name = request.Name,
                Description = request.Description,
            };

            _ = await _featureRepository.AddAsync(feature);
            await _featureRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(feature.Id.ToString());
        }

        public async Task<ApiResponse<string>> DeleteFeatureAsync(Guid id)
        {
            var feature = await _featureRepository.GetByIdAsync(id);

            if (feature == null)
            {
                return ApiResponse<string>.Fail(messageId: MessageId.E0005);
            }

            await _featureRepository.SoftDeleteAsync(feature);
            await _featureRepository.SaveChangesAsync();

            return ApiResponse<string>.Ok(id.ToString());
        }

        public async Task<ApiResponse<List<GetFeatureResponse>>> GetAllFeaturesAsync()
        {
            var features = await _featureRepository.GetAllAsync();

            if (!features.Any())
            {
                return ApiResponse<List<GetFeatureResponse>>.Fail(MessageId.E0005);
            }

            var response = features.Select(x => new GetFeatureResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
            }).ToList();

            return ApiResponse<List<GetFeatureResponse>>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetFeatureResponse>>> GetFeaturesAsync(PagingRequest request)
        {
            var response = await _featureRepository.FindPagedAsync(
                predicate: query => !query.IsDeleted,
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                include: null,
                orderBy: query => query.OrderByDescending(x => x.CreatedAt));

            var responseConverted = new PagingResponse<GetFeatureResponse>
            {
                Items = response.Items.Select(x => new GetFeatureResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                }),
                TotalItems = response.TotalItems,
                PageIndex = response.PageIndex,
                PageSize = response.PageSize,
            };

            return ApiResponse<PagingResponse<GetFeatureResponse>>.Ok(responseConverted);
        }
    }
}
