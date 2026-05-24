using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(User user);
        string GenerateRefreshToken();
        Task<ResponseMessage<LoginUserResponse>> RefreshToken(string refreshToken, CancellationToken cancellationToken);
        Task SaveRefreshToken(RefreshToken refreshToken);
        void RevokeAllUserRefreshTokens(Guid userId);
    }
}
