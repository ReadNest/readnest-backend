using ReadNest.Application.Models.Requests.Package;
using ReadNest.Application.Models.Responses.Package;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Package
{
    public interface IPackageUseCase
    {
        Task<ApiResponse<PagingResponse<GetPackageResponse>>> GetPackgesWithPagingAsync(PagingRequest request);
        Task<ApiResponse<string>> CreatePackageAsync(CreatePackageRequest request);
        //Task<ApiResponse<string>> UpdatePackageAsync();
        Task<ApiResponse<string>> DeletePackageAsync(Guid id);
    }
}
