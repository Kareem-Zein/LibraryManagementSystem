using Microsoft.EntityFrameworkCore.Storage;

namespace LibraryManagementSystem.Application.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task<bool> ExecuteInTransactionAsync(Func<CancellationToken, Task<bool>> func, CancellationToken cancellationToken);
    }
}
