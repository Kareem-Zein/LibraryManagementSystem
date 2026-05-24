using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Application.Interfaces.Repositories
{
    public interface IRefreshTokenRepository : IRepositoryBase<RefreshToken>
    {
        Task<RefreshToken?> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken);
        void RevokeAllUserRefreshTokens(Guid userId);
    }
}
