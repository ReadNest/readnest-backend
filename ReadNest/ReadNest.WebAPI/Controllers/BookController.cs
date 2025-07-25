﻿using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Book;
using ReadNest.Application.Models.Responses.Book;
using ReadNest.Application.Models.Responses.TradingPost;
using ReadNest.Application.UseCases.Interfaces.Book;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v1/books")]
    [AllowAnonymous]
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
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Substring("Bearer ".Length).Trim() ?? string.Empty;

            var response = await _bookUseCase.SearchBooksAsync(paging, keyword, token);
            return Ok(response);
        }

        [HttpGet("filter")]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetBookSearchResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> FilterBooks([FromQuery] BookFilterRequest request)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Substring("Bearer ".Length).Trim() ?? string.Empty;

            var response = await _bookUseCase.FilterBooksAsync(request, token);
            return Ok(response);
        }


        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetBookById([FromRoute] Guid bookId)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = authHeader?.Substring("Bearer ".Length).Trim() ?? string.Empty;

            var response = await _bookUseCase.GetByIdAsync(bookId, token);
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpGet("all")]
        [ProducesResponseType(typeof(ApiResponse<List<GetBookTradingPostResponse>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllBookWithoutPaging()
        {
            var response = await _bookUseCase.GetBookTradingPostAsync();
            return response.Success ? Ok(response) : NotFound(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateBook([FromBody] CreateBookRequest request)
        {
            var response = await _bookUseCase.CreateBookAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPut("{bookId}")]
        [ProducesResponseType(typeof(ApiResponse<GetBookResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateBook([FromRoute] Guid bookId, [FromBody] UpdateBookRequest request)
        {
            var response = await _bookUseCase.UpdateBookAsync(bookId, request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteBook([FromRoute] Guid id)
        {
            var response = await _bookUseCase.DeleteBookAsync(id);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
