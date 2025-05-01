using ReadNest.Application.Models.Requests.Auth;
using ReadNest.Application.Models.Responses.Auth;
using ReadNest.Shared.Common;

namespace ReadNest.Application.UseCases.Interfaces.Auth
{
    public interface IAuthenticationUseCase
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterRequest request);
        Task<ApiResponse<TokenResponse>> LoginAsync(LoginRequest request);
    }
}
