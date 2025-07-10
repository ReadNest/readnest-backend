using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;

namespace ReadNest.Application.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreatePaymentLinkAsync(Transaction transaction, Package package, User user);
        Task<string> InitWebhook();
    }
}
