using FluentValidation;
using ReadNest.Application.Models.Requests.Payment;
using ReadNest.Application.Repositories;

namespace ReadNest.Application.Validators.Transaction
{
    public class CreatePaymentLinkRequestValidator : AbstractValidator<CreatePaymentLinkRequest>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPackageRepository _packageRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="packageRepository"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CreatePaymentLinkRequestValidator(IUserRepository userRepository, IPackageRepository packageRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _packageRepository = packageRepository ?? throw new ArgumentNullException(nameof(packageRepository));

            RuleFor(x => x.UserId)
                .MustAsync(async (userId, cancellation) =>
                    await _userRepository.GetByUserIdAsync(userId) != null)
                .WithMessage("Không tìm thấy user.");

            RuleFor(x => x.PackageId)
                .MustAsync(async (packageId, cancellation) =>
                    await _packageRepository.GetByIdAsync(packageId) != null)
                .WithMessage("Không tìm thấy package.");
        }
    }

}
