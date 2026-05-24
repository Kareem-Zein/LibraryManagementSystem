using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public RefreshTokenRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<RefreshToken?> GetByTokenAsync(string refreshToken, CancellationToken cancellationToken)
        {
            return await DbContext.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);
        }

        public void RevokeAllUserRefreshTokens(Guid userId)
        {
            var refreshTokens = DbContext.RefreshTokens.Where(r => r.UserId == userId && r.IsRevoked == false).ToList();
            refreshTokens.ForEach(rt => rt.IsRevoked = true);
            DbContext.UpdateRange(refreshTokens);
        }
    }
}
