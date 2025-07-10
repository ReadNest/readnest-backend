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
        private readonly PayOS _payos;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        public PayOSPaymentGateway(IOptions<PayOSOptions> options)
        {
            _options = options.Value;
            _payos = new PayOS(
                apiKey: _options.ApiKey,
                clientId: _options.ClientId,
                checksumKey: _options.ChecksumKey
            );
        }

        public async Task<string> CreatePaymentLinkAsync(Domain.Entities.Transaction transaction, Package package, User user)
        {
            var response = await _payos.createPaymentLink(new PaymentData
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

        public async Task<string> InitWebhookPayOSAsync()
        {
            var result = await _payos.confirmWebhook(_options.WebhookUrl);
            return result;
        }

        public WebhookData VerifyWebHook(WebhookType webhookType)
        {
            return _payos.verifyPaymentWebhookData(webhookType);
        }
    }
}
