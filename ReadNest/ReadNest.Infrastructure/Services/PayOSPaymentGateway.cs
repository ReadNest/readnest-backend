using Microsoft.Extensions.Options;
using Net.payOS;
using Net.payOS.Types;
using ReadNest.Application.Services;
using ReadNest.Domain.Entities;
using ReadNest.Infrastructure.Options;

namespace ReadNest.Infrastructure.Services
{
    public class PayOSPaymentGateway : IPaymentGateway
    {
        private readonly PayOSOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public PayOSPaymentGateway(IOptions<PayOSOptions> options)
        {
            _options = options.Value;
        }

        public async Task<string> CreatePaymentLinkAsync(Domain.Entities.Transaction transaction, Package package, User user)
        {
            var payos = new PayOS(
                apiKey: _options.ApiKey,
                clientId: _options.ClientId,
                checksumKey: _options.ChecksumKey
            );

            var response = await payos.createPaymentLink(new PaymentData
            (
                orderCode: transaction.OrderCode,
                amount: Convert.ToInt16(package.Price),
                description: $"Thanh toan {package.Name}",
                items: new List<ItemData>
                {
                    new(name: package.Name, quantity: 1, price: Convert.ToInt32(package.Price))
                },
                cancelUrl: _options.CancelUrl,
                returnUrl: _options.ReturnUrl,
                buyerName: user.FullName,
                buyerEmail: user.Email,
                buyerAddress: user.Address,
                expiredAt: new DateTimeOffset(DateTime.UtcNow.AddMinutes(15)).ToUnixTimeSeconds()
            ));

            return response.checkoutUrl;
        }

        public async Task<string> InitWebhook()
        {
            var payos = new PayOS(
                apiKey: _options.ApiKey,
                clientId: _options.ClientId,
                checksumKey: _options.ChecksumKey
            );

            var result = await payos.confirmWebhook(_options.WebhookUrl);
            return result;
        }
    }
}
