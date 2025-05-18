using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.AffiliateLink;
using ReadNest.Application.UseCases.Interfaces.AffiliateLink;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/books/{bookId}/affiliate-links")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AffiliateLinkController : ControllerBase
    {
        private readonly IAffiliateLinkUseCase _affiliateLinkUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="affiliateLinkUseCase"></param>
        public AffiliateLinkController(IAffiliateLinkUseCase affiliateLinkUseCase)
        {
            _affiliateLinkUseCase = affiliateLinkUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAffiliateLink([FromRoute] Guid bookId, [FromBody] CreateAffiliateLinkRequest request)
        {
            var response = await _affiliateLinkUseCase.CreateAsync(bookId, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
