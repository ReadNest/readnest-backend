using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.CommentReport;
using ReadNest.Application.UseCases.Interfaces.Comment;
using ReadNest.Application.UseCases.Interfaces.CommentReport;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentReportController : ControllerBase
    {
        private readonly ICommentReportUseCase _commentReportUseCase;
        public CommentReportController(ICommentReportUseCase commentReportUseCase)
        {
            _commentReportUseCase = commentReportUseCase;
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCommentReport([FromBody] CreateCommentReportRequest request)
        {
            var response = await _commentReportUseCase.CreateCommentReportAsync(request);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("approve/{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ApproveCommentReportAndBan(Guid commentId)
        {
            var response = await _commentReportUseCase.ApproveCommentReportAndBan(commentId);
            return response.Success ? Ok(response) : BadRequest(response);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("reject/{commentId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RejectCommentReport(Guid commentId)
        {
            var response = await _commentReportUseCase.RejectCommentReport(commentId);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
