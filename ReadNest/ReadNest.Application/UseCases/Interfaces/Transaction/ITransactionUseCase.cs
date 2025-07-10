using ReadNest.Application.Models.Requests.Payment;
using ReadNest.Application.Models.Responses.Payment;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Transaction
{
    public interface ITransactionUseCase
    {
        Task<Guid> CreateTransactionAsync(CreatePaymentLinkRequest request);
        Task<ApiResponse<GetPaymentLinkResponse>> GetPaymentLinkResponseAsync(
            Guid transactionId,
            Guid packageId,
            Guid userId);
        Task<ApiResponse<string>> InitWebhookPayOS();
    }
}
