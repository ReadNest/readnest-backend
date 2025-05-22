using System.Net;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.FavoriteBook;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.FavoriteBook;
using ReadNest.Application.UseCases.Interfaces.FavoriteBook;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/favoriteBooks")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FavoriteBookController : ControllerBase
    {
        private readonly IFavoriteBookUseCase _favoriteBookUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="favoriteBookUseCase"></param>

        public FavoriteBookController(IFavoriteBookUseCase favoriteBookUseCase)
        {
            _favoriteBookUseCase = favoriteBookUseCase;
        }

        [HttpPost("toggle")]
        [ProducesResponseType(typeof(ApiResponse<ToggleFavoriteBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ToggleFavorite([FromBody] ToggleFavoriteBookRequest request)
        {
            var response = await _favoriteBookUseCase.ToggleFavoriteAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpGet("favorites/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetFavoriteBooksPagedByUser([FromRoute] Guid userId, [FromQuery] PagingRequest request)
        {
            var response = await _favoriteBookUseCase.GetFavoriteBooksPagedByUserAsync(userId, request);
            return response.Success ? Ok(response) : NotFound(response);
        }
    }
}
