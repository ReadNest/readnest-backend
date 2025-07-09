using ReadNest.Domain.Entities;

namespace ReadNest.Application.Services
{
    public interface IPaymentGateway
    {
        Task<string> CreatePaymentLinkAsync(Transaction transaction, Package package);
    }
}
