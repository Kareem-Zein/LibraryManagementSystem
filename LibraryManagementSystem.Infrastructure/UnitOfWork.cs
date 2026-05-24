using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagementSystem.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _dbContext;

        public UnitOfWork(LibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _dbContext.SaveChangesAsync(cancellationToken);

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) => await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        public async Task<bool> ExecuteInTransactionAsync(Func<CancellationToken, Task<bool>> func, CancellationToken cancellationToken)
        {
            using (var transaction = await BeginTransactionAsync(cancellationToken))
            {
                try
                {
                    var result = await func(cancellationToken);

                    if (result)
                    {
                        await transaction.CommitAsync(cancellationToken);
                        await SaveChangesAsync(cancellationToken);
                    }

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
            }

            return false;
        }
    }
}
