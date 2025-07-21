using System.Net;
using Microsoft.AspNetCore.Mvc;
using Net.payOS.Types;
using ReadNest.Application.Models.Requests.Payment;
using ReadNest.Application.Models.Responses.Payment;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Transaction;
using ReadNest.Shared.Common;

namespace ReadNest.WebAPI.Controllers
{
    [Route("api/v1/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ITransactionUseCase _transactionUseCase;

        public PaymentController(IPaymentGateway paymentGateway,
            ITransactionUseCase transactionUseCase)
        {
            _transactionUseCase = transactionUseCase;
        }

        [HttpPost("payment-links")]
        [ProducesResponseType(typeof(ApiResponse<GetPaymentLinkResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentLinkRequest request)
        {
            var transactionId = await _transactionUseCase.CreateTransactionAsync(request);
            var response = await _transactionUseCase.GetPaymentLinkResponseAsync(transactionId, request.PackageId, request.UserId);

            return response.Success ? Ok(response) : BadRequest(response);
        }

        [HttpPost("webhook")]
        [ProducesResponseType(typeof(ApiResponse<string>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> HandleWebhookPayOS([FromBody] WebhookType request)
        {
            var webHookData =  _transactionUseCase.VerifyPaymentWebhookData(request);
            var response = await _transactionUseCase.CreateUserSubscription(webHookData.Data);
            return Ok(response);
        }

        [HttpPost("init-webhook")]
        public async Task<IActionResult> ConfirmWebhook()
        {
            var response = await _transactionUseCase.InitWebhookPayOSAsync();
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
