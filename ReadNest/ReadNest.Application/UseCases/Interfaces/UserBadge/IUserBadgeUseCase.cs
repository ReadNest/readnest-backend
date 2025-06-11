using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.UserBadge
{
    public interface IUserBadgeUseCase
    {
        public Task<ApiResponse<string>> AssignBadgeToAllUsers(string badgeCode);
        public Task<ApiResponse<string>> SelectUserBadge(Guid userId, Guid badgeId);
        public Task<ApiResponse<string>> SetAllBadgesActiveByBadgeCode(string badgeCode);
    }
}
