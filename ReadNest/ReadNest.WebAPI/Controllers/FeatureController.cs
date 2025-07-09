using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Feature;
using ReadNest.Application.Models.Responses.Feature;
using ReadNest.Application.UseCases.Interfaces.Feature;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/features")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class FeatureController : ControllerBase
    {
        private readonly IFeatureUseCase _featureUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="featureUseCase"></param>
        public FeatureController(IFeatureUseCase featureUseCase)
        {
            _featureUseCase = featureUseCase;
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<GetFeatureResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllFeatures()
        {
            var response = await _featureUseCase.GetAllFeaturesAsync();
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetFeatureResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFeaturesWithPaging([FromQuery] PagingRequest request)
        {
            var response = await _featureUseCase.GetFeaturesAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateFeature([FromBody] CreateFeatureRequest request)
        {
            var response = await _featureUseCase.CreateFeatureAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteFeature([FromRoute] Guid id)
        {
            var response = await _featureUseCase.DeleteFeatureAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
