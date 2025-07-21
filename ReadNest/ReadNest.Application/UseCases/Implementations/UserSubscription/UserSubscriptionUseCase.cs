using Microsoft.EntityFrameworkCore;
using ReadNest.Application.Models.Responses.UserSubscription;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.UserSubscription;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.UserSubscription
{
    public class UserSubscriptionUseCase : IUserSubscriptionUseCase
    {
        private readonly IUserSubscriptionRepository _repository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        public UserSubscriptionUseCase(IUserSubscriptionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ApiResponse<GetUserSubscriptionResponse>> GetUserSubscriptionByUserIdAsync(Guid userId)
        {
            var userSubscription = (await _repository.FindWithIncludeAsync(
                predicate: query => !query.IsDeleted && query.UserId == userId,
                orderBy: query => query.OrderByDescending(x => x.UpdatedAt),
                include: query => query.Include(x => x.Package)))
                .Select(x => new GetUserSubscriptionResponse(
                    x.StartDate,
                    x.EndDate,
                    x.Status,
                    x.Package.Name))
                .FirstOrDefault();

            if (userSubscription == null)
            {
                return ApiResponse<GetUserSubscriptionResponse>.Fail(messageId: MessageId.E0005);
            }

            return ApiResponse<GetUserSubscriptionResponse>.Ok(userSubscription);
        }
    }
}
