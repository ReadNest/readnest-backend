using ReadNest.Application.Models.Requests.Category;
using ReadNest.Application.Models.Responses.Category;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Category
{
    public interface ICategoryUseCase
    {
        Task<ApiResponse<GetCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request);
        Task<ApiResponse<GetCategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<ApiResponse<PagingResponse<GetCategoryResponse>>> GetAllAsync(PagingRequest request);
        Task<ApiResponse<List<GetCategoryResponse>>> GetAllCategoriesAsync();
    }
}
