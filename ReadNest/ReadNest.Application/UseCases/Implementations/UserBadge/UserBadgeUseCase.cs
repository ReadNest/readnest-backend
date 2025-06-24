using ReadNest.Application.Repositories;
using ReadNest.Application.UseCases.Interfaces.UserBadge;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Implementations.UserBadge
{
    public class UserBadgeUseCase : IUserBadgeUseCase
    {
        private readonly IUserBadgeRepository _userBadgeRepository;
        private readonly IBadgeRepository _badgeRepository;
        private readonly IUserRepository _userRepository;
        public UserBadgeUseCase(IUserBadgeRepository userBadgeRepository, IBadgeRepository badgeRepository, IUserRepository userRepository)
        {
            _userBadgeRepository = userBadgeRepository;
            _badgeRepository = badgeRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResponse<string>> AssignBadgeToAllUsers(string badgeCode)
        {
            var badge = await _badgeRepository.GetByCodeAsync(badgeCode);
            if (badge is null)
            {
                return ApiResponse<string>.Fail("Badge code does not exist!");
            }
            var users = await _userRepository.GetAllUsersWithRoleUserAsync();
            if (users is null || !users.Any())
            {
                return ApiResponse<string>.Fail("No users found to assign the badge.");
            }
            // Kiểm tra xem có người dùng nào đã có badge này chưa
            var existingUserBadges = await _userBadgeRepository.GetByBadgeIdAsync(badge.Id);
            // Nếu có người dùng đã có badge này, xóa người đó khỏi list users
            if (existingUserBadges.Any())
            {
                users = users.Where(u => !existingUserBadges.Any(ub => ub.UserId == u.Id)).ToList();
            }
            if (!users.Any())
            {
                return ApiResponse<string>.Ok("All users already have this badge.");
            }
            var userBadges = users.Select(users => new Domain.Entities.UserBadge
            {
                UserId = users.Id,
                BadgeId = badge.Id,
                IsSelected = false,
            }).ToList();
            var result = await _userBadgeRepository.AddRangeAsync(userBadges);
            if (result.Count() != users.Count())
            {
                await _userBadgeRepository.DeleteRangeAsync(userBadges);
                return ApiResponse<string>.Fail("Failed to assign badge to all users. Some users may already have this badge.");
            }
            await _userBadgeRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok(string.Format("Badge assigned to {0} user(s) successfully.", result.Count()));

        }

        public async Task<ApiResponse<string>> SelectUserBadge(Guid userId, Guid badgeId)
        {
            var userBadges = await _userBadgeRepository.GetAvailableByUserIdAsync(userId);
            var selectedBadge = userBadges.FirstOrDefault(ub => ub.BadgeId == badgeId);
            if (selectedBadge is null)
            {
                return ApiResponse<string>.Fail("This badge is not owned by the user.");
            }
            if (selectedBadge.IsSelected)
            {
                return ApiResponse<string>.Ok("This badge is already selected by the user.");
            }
            // Gỡ tất cả các badge đang được chọn về false
            foreach (var ub in userBadges)
            {
                ub.IsSelected = false;
            }
            selectedBadge.IsSelected = true;
            await _userBadgeRepository.UpdateAsync(selectedBadge);
            await _userBadgeRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("Badge selected successfully.");
        }

        public async Task<ApiResponse<string>> SetAllBadgesActiveByBadgeCode(string badgeCode)
        {
            // Gỡ tất cả các badge về false
            var userBadges = await _userBadgeRepository.GetAllAsync();
            if (userBadges is null || !userBadges.Any())
            {
                return ApiResponse<string>.Fail("No user badges found.");
            }
            foreach (var ub in userBadges)
            {
                ub.IsSelected = false;
            }
            // Set tất cả các badge theo badgeCode về true
            var defaultBadges = await _userBadgeRepository.GetAvailableByBadgeCodeAsync(badgeCode);
            if (defaultBadges is null || !defaultBadges.Any())
            {
                return ApiResponse<string>.Fail("No default badges found.");
            }
            foreach (var db in defaultBadges)
            {
                db.IsSelected = true;
            }
            await _userBadgeRepository.UpdateRangeAsync(defaultBadges);
            await _userBadgeRepository.SaveChangesAsync();
            return ApiResponse<string>.Ok("All default badges set to active successfully.");
        }
    }
}
