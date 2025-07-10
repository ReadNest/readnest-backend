using Net.payOS.Types;
using ReadNest.Domain.Entities;

namespace ReadNest.Application.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreatePaymentLinkAsync(Domain.Entities.Transaction transaction, Package package, User user);
        Task<string> InitWebhookPayOSAsync();
        WebhookData VerifyWebHook(WebhookType webhookType);
    }
}
