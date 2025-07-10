using FluentValidation;
using ReadNest.Application.Models.Requests.Payment;
using ReadNest.Application.Models.Responses.Payment;
using ReadNest.Application.Repositories;
using ReadNest.Application.Services;
using ReadNest.Application.UseCases.Interfaces.Transaction;
using ReadNest.Application.Validators.Transaction;
using ReadNest.Shared.Common;
using ReadNest.Shared.Enums;

namespace ReadNest.Application.UseCases.Implementations.Transaction
{
    public class TransactionUseCase : ITransactionUseCase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPackageRepository _packageRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IPaymentGateway _paymentGateway;
        private readonly CreatePaymentLinkRequestValidator _createValidator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="packageRepository"></param>
        /// <param name="transactionRepository"></param>
        /// <param name="paymentGateway"></param>
        /// <param name="createValidator"></param>
        public TransactionUseCase(
            IUserRepository userRepository,
            IPackageRepository packageRepository,
            ITransactionRepository transactionRepository,
            IPaymentGateway paymentGateway,
            CreatePaymentLinkRequestValidator createValidator)
        {
            _packageRepository = packageRepository;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
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

        public async Task<ApiResponse<GetPaymentLinkResponse>> GetPaymentLinkResponseAsync(
            Guid transactionId,
            Guid packageId,
            Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            var package = await _packageRepository.GetByIdAsync(packageId);
            var transaction = await _transactionRepository.GetByIdAsync(transactionId);

            var paymentLink = await _paymentGateway.CreatePaymentLinkAsync(transaction, package, user);

            if(string.IsNullOrEmpty(paymentLink))
            {
                return ApiResponse<GetPaymentLinkResponse>.Fail(MessageId.E0000);
            }

            return ApiResponse<GetPaymentLinkResponse>.Ok(new GetPaymentLinkResponse { CheckoutUrl = paymentLink });
        }

        public async Task<ApiResponse<string>> InitWebhookPayOS()
        {
            return ApiResponse<string>.Ok(await _paymentGateway.InitWebhook());
        }
    }
}
