using System.Security.Claims;

namespace ReadNest.Application.Services
{
    public interface IJwtService
    {
        string GenerateAccessToken(Guid userId, string role);
        Task<string> GenerateRefreshTokenAsync(Guid userId);
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
        Task<bool> ValidateRefreshTokenAsync(Guid userId, string refreshToken);
    }
}
