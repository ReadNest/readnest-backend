using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Category;
using ReadNest.Application.Models.Responses.Category;
using ReadNest.Application.UseCases.Interfaces.Category;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/categories")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryUseCase _categoryUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="categoryUseCase"></param>

        public CategoryController(ICategoryUseCase categoryUseCase)
        {
            _categoryUseCase = categoryUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetCategoryResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var response = await _categoryUseCase.GetAllAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetCategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var response = await _categoryUseCase.CreateCategoryAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<GetCategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            var response = await _categoryUseCase.UpdateCategoryAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
