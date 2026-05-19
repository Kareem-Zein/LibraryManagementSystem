using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Application.Interfaces.Repositories;

public interface IRepositoryBase<T> where T : EntityBase
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default);

    void Update(T entity);
    void Delete(T entity);
}
