using ReadNest.Application.Models.Requests.Feature;
using ReadNest.Application.Models.Responses.Feature;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Feature
{
    public interface IFeatureUseCase
    {
        Task<ApiResponse<string>> CreateFeatureAsync(CreateFeatureRequest request);
        Task<ApiResponse<string>> DeleteFeatureAsync(Guid id);
        Task<ApiResponse<PagingResponse<GetFeatureResponse>>> GetFeaturesAsync(PagingRequest request);
    }
}
