using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Package;
using ReadNest.Application.Models.Responses.Package;
using ReadNest.Application.UseCases.Interfaces.Package;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/packages")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackageUseCase _packageUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="packageUseCase"></param>
        public PackageController(IPackageUseCase packageUseCase)
        {
            _packageUseCase = packageUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetPackageResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetPackagesWithPaging([FromQuery] PagingRequest request)
        {
            var response = await _packageUseCase.GetPackgesWithPagingAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request)
        {
            var response = await _packageUseCase.CreatePackageAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeletePackage([FromRoute] Guid id)
        {
            var response = await _packageUseCase.DeletePackageAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
