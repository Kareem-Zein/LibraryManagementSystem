using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly LibraryDbContext DbContext;

        protected BaseRepository(LibraryDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await DbContext.Set<T>().AddAsync(entity, cancellationToken);

        public void Delete(T entity) => DbContext.Set<T>().Remove(entity);

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default) => await DbContext.Set<T>().AnyAsync(e => e.Id == id, cancellationToken);

        public void Update(T entity) => DbContext.Set<T>().Update(entity);
    }
}
