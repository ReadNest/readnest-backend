using ReadNest.Application.Models.Requests.User;
using ReadNest.Application.Models.Responses.User;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.User
{
    public interface IUserUseCase
    {
        Task<ApiResponse<string>> CreateUserAsync(CreateUserRequest request);
        Task<ApiResponse<string>> UpdateProfileAsync(Guid userId, UpdateUserRequest request);
        Task<ApiResponse<string>> ChangePasswordAsync(Guid userId, ChangePasswordRequest request);
        Task<ApiResponse<string>> DeleteAccountAsync(Guid userId);
        Task<ApiResponse<PagingResponse<GetUserResponse>>> GetAllAsync(PagingRequest request);
        Task<ApiResponse<GetUserResponse>> GetByIdAsync(Guid userId);
        Task<ApiResponse<GetUserProfileResponse>> GetByUserNameAsync(string userName);
    }
}
