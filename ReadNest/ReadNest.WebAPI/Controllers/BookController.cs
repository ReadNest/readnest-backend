using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : ControllerBase
    {
        private readonly IBookUseCase _bookUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="bookUseCase"></param>
        public BookController(IBookUseCase bookUseCase)
        {
            _bookUseCase = bookUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll([FromQuery] PagingRequest request)
        {
            var response = await _bookUseCase.GetAllAsync(request);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookSearchResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> SearchBooks([FromQuery] PagingRequest paging, [FromQuery] string? keyword)
        {
            var response = await _bookUseCase.SearchBooksAsync(paging, keyword);
            return Ok(response);
        }


        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBookById([FromRoute] Guid bookId)
        {
            var response = await _bookUseCase.GetByIdAsync(bookId);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            var response = await _bookUseCase.CreateBookAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
