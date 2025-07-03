
using FluentValidation;
using ReadNest.Application.Models.Requests.Category;
using ReadNest.Application.Models.Responses.Category;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Category;
using ReadNest.Application.Validators.Category;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Category
{
    public class CategoryUseCase : ICategoryUseCase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly CreateCategoryRequestValidator _validator;

        public CategoryUseCase(ICategoryRepository categoryRepository, CreateCategoryRequestValidator validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        public async Task<ApiResponse<GetCategoryResponse>> CreateCategoryAsync(CreateCategoryRequest request)
        {
            await _validator.ValidateAndThrowAsync(request);
            var category = new Domain.Entities.Category
            {
                Name = request.Name,
                Description = request.Description
            };

            _ = await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return ApiResponse<GetCategoryResponse>.Ok(response);
        }

        public async Task<ApiResponse<GetCategoryResponse>> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            //await _validator.ValidateAndThrowAsync(request);
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null || category.IsDeleted)
            {
                return ApiResponse<GetCategoryResponse>.Fail(MessageId.E0005);
            }

            category.Name = request.Name;
            category.Description = request.Description;

            await _categoryRepository.UpdateAsync(category);
            await _categoryRepository.SaveChangesAsync();

            var response = new GetCategoryResponse
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };

            return ApiResponse<GetCategoryResponse>.Ok(response);
        }

        public async Task<ApiResponse<PagingResponse<GetCategoryResponse>>> GetAllAsync(PagingRequest request)
        {
            var pagingResponse = await _categoryRepository.FindPagedAsync(
                predicate: query => !query.IsDeleted,
                pageNumber: request.PageIndex,
                pageSize: request.PageSize,
                include: null,
                asNoTracking: true,
                orderBy: query => query.OrderByDescending(x => x.CreatedAt)
                                       .ThenByDescending(x => x.UpdatedAt)
                                       .ThenByDescending(x => x.Name)
            );

            if (pagingResponse.TotalItems == 0)
            {
                return ApiResponse<PagingResponse<GetCategoryResponse>>.Fail(MessageId.E0005);
            }

            var data = new PagingResponse<GetCategoryResponse>
            {
                Items = pagingResponse.Items.Select(x => new GetCategoryResponse
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                }).ToList(),
                TotalItems = await _categoryRepository.CountAsync(x => !x.IsDeleted),
                PageIndex = pagingResponse.PageIndex,
                PageSize = pagingResponse.PageSize
            };

            return ApiResponse<PagingResponse<GetCategoryResponse>>.Ok(data);
        }

        public async Task<ApiResponse<List<GetCategoryResponse>>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            if (categories == null || !categories.Any())
            {
                return ApiResponse<List<GetCategoryResponse>>.Fail(MessageId.E0005);
            }

            var response = categories.Select(x => new GetCategoryResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description
            }).ToList();

            return ApiResponse<List<GetCategoryResponse>>.Ok(response, MessageId.I0000);
        }
    }
}
