using FluentValidation;
using Net.payOS.Types;
using ReadNest.Application.Models.Requests.Payment;
using ReadNest.Application.Models.Responses.Payment;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Transaction;
using ReadNest.Application.Validators.Transaction;
using ReadNest.Domain.Entities;
using ReadNest.Shared.Common;
using ReadNest.Shared.Enums;

namespace ReadNest.Application.UseCases.Implementations.Transaction
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly CreatePaymentLinkRequestValidator _createValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="packageRepository"></param>
        /// <param name="transactionRepository"></param>
        /// <param name="userSubscriptionRepository"></param>
        /// <param name="paymentGateway"></param>
        /// <param name="createValidator"></param>
        public TransactionUseCase(
            IUserRepository userRepository,
            IPackageRepository packageRepository,
            ITransactionRepository transactionRepository,
            IUserSubscriptionRepository userSubscriptionRepository,
            IPaymentGateway paymentGateway,
            CreatePaymentLinkRequestValidator createValidator)
        {
            _packageRepository = packageRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _userSubscriptionRepository = userSubscriptionRepository;
            _paymentGateway = paymentGateway;
            _createValidator = createValidator;
        }

        public async Task<Guid> CreateTransactionAsync(CreatePaymentLinkRequest request)
        {
            await _createValidator.ValidateAndThrowAsync(request);

            var package = await _packageRepository.GetByIdAsync(request.PackageId);
            var orderCode = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            var transaction = new Domain.Entities.Transaction
            {
                UserId = request.UserId,
                PackageId = request.PackageId,
                OrderCode = orderCode,
                Amount = package.Price,
                PaymentMethod = PaymentMethodEnum.PayOS.ToString(),
                TransactionStatus = StatusEnum.Pending.ToString(),
            };

            _ = await _transactionRepository.AddAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            return transaction.Id;
        }

        public async Task<ApiResponse<string>> CreateUserSubscription(WebhookData webhookData)
        {
            var transaction = await _transactionRepository.GetByOrderCodeAsync(webhookData.orderCode);
            if (transaction == null)
            {
                return ApiResponse<string>.Fail(MessageId.E0005);
            }

            if (transaction.TransactionStatus == StatusEnum.Success.ToString())
            {
                return ApiResponse<string>.Ok("Already handled");
            }

            transaction.TransactionStatus = StatusEnum.Success.ToString();
            await _transactionRepository.UpdateAsync(transaction);
            await _transactionRepository.SaveChangesAsync();

            var subscription = new UserSubscription
            {
                UserId = transaction.UserId,
                PackageId = transaction.PackageId,
                StartDate = DateTime.UtcNow,
                Status = StatusEnum.Active.ToString()
            };

            _ = await _userSubscriptionRepository.AddAsync(subscription);

            await _transactionRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(subscription.Id.ToString());
        }

        public async Task<ApiResponse<GetPaymentLinkResponse>> GetPaymentLinkResponseAsync(
            Guid transactionId,
            Guid packageId,
            Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var package = await _packageRepository.GetByIdAsync(packageId);
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);

            var paymentLink = await _paymentGateway.CreatePaymentLinkAsync(transaction, package, user);

            if (string.IsNullOrEmpty(paymentLink))
            {
                return ApiResponse<GetPaymentLinkResponse>.Fail(MessageId.E0000);
            }

            return ApiResponse<GetPaymentLinkResponse>.Ok(new GetPaymentLinkResponse { CheckoutUrl = paymentLink });
        }

        public async Task<ApiResponse<string>> InitWebhookPayOSAsync()
        {
            return ApiResponse<string>.Ok(await _paymentGateway.InitWebhookPayOSAsync());
        }

        ApiResponse<WebhookData> ITransactionUseCase.VerifyPaymentWebhookData(WebhookType webHookType)
        {
            var response = _paymentGateway.VerifyWebHook(webHookType);
            return ApiResponse<WebhookData>.Ok(response);
        }
    }
}
