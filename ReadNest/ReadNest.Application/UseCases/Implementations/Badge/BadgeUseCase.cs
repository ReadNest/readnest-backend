using ReadNest.Application.Models.Requests.Badge;
using ReadNest.Application.Models.Responses.Badge;
using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.Badge;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.Badge
{
    public class BadgeUseCase : IBadgeUseCase
    {
        private readonly IBadgeRepository _badgeRepository;
        public BadgeUseCase(IBadgeRepository badgeRepository)
        {
            _badgeRepository = badgeRepository;
        }

        public async Task<ApiResponse<CreateBadgeResponse>> CreateBadgeAsync(CreateBadgeRequest request)
        {
            // Check if this badge code already exists?
            var existingBadge = await _badgeRepository.GetByCodeAsync(request.Code);
            if (existingBadge != null)
            {
                return ApiResponse<CreateBadgeResponse>.Fail("Badge with this code already exists.");
            }
            // Create a new badge
            var badge = new Domain.Entities.Badge
            {
                Code = request.Code,
                Name = request.Name,
                Description = request.Description,
            };
            // Save the badge to the database
            _ = await _badgeRepository.AddAsync(badge);
            await _badgeRepository.SaveChangesAsync();
            return ApiResponse<CreateBadgeResponse>.Ok(new CreateBadgeResponse
            {
                Code = badge.Code,
                Name = badge.Name,
                Description = badge.Description
            });
        }

        public async Task<ApiResponse<string>> SoftDeleteBadgeByCodeAsync(string code)
        {
            // Check if the badge exists
            var badge = await _badgeRepository.GetByCodeAsync(code);
            if (badge == null)
            {
                return ApiResponse<string>.Fail("Badge not found.");
            }
            // Delete the badge
            await _badgeRepository.SoftDeleteAsync(badge);
            await _badgeRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Badge deleted successfully.");
        }

        public async Task<ApiResponse<List<GetBadgeResponse>>> GetBadgesAsync()
        {
            // Get all badges from the repository
            var badges = await _badgeRepository.GetAllAvailableAsync();
            // Map the badges to the response model
            var response = badges.Select(b => new GetBadgeResponse
            {
                Id = b.Id,
                Code = b.Code,
                Name = b.Name,
                Description = b.Description
            }).ToList();
            return ApiResponse<List<GetBadgeResponse>>.Ok(response);
        }

        public async Task<ApiResponse<string>> UpdateBadgeAsync(UpdateBadgeRequest request)
        {
            // Check if the badge exists
            var badge = await _badgeRepository.GetByCodeAsync(request.Code);
            if (badge == null)
            {
                return ApiResponse<string>.Fail("Badge not found.");
            }

            // Check if this badge code already exists?
            var existingBadge = await _badgeRepository.GetByCodeAsync(request.Code);
            if (existingBadge != null)
            {
                return ApiResponse<string>.Fail("Badge with this code already exists.");
            }

            // Update the badge properties
            badge.Code = request.Code;
            badge.Name = request.Name;
            badge.Description = request.Description;
            // Save changes to the repository
            await _badgeRepository.UpdateAsync(badge);
            await _badgeRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Badge updated successfully.");
        }
    }
}
