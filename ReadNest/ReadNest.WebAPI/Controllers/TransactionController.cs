using System.Net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadNest.Application.Models.Requests.Transaction;
using ReadNest.Application.Models.Responses.Transaction;
using ReadNest.Application.UseCases.Interfaces.Transaction;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/users/{userId}/transactions")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User,Admin")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="transactionUseCase"></param>
        public TransactionController(ITransactionUseCase transactionUseCase)
        {
            _transactionUseCase = transactionUseCase;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<PagingResponse<GetTransactionResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTransactionsByUserId([FromRoute] Guid userId, [FromQuery] GetTransactionRequest request)
        {
            var transactions = await _transactionUseCase.GetTransactionsByUserIdAsync(userId, request);
            return Ok(transactions);
        }
    }
}
