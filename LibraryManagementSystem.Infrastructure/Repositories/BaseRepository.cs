using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryManagementSystem.Infrastructure.Repositories
{
    public class BaseRepository<T> : IRepositoryBase<T> where T : EntityBase
    {
        protected readonly LibraryDbContext DbContext;

        protected BaseRepository(LibraryDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = DbContext.Set<T>();

            foreach (var include in includes)
                query = query.Include(include);

            var entityType = DbContext.Model.FindEntityType(typeof(T));

            var primaryKey = entityType?.FindPrimaryKey()?.Properties.FirstOrDefault();

            if (primaryKey is null)
                throw new Exception($"No primary key found for {typeof(T).Name}");

            var keyName = primaryKey.Name;

            return await query.FirstOrDefaultAsync(e => EF.Property<object>(e, keyName).Equals(id));
        }
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default) => await DbContext.Set<T>().AddAsync(entity, cancellationToken);

        public void Delete(T entity) => DbContext.Set<T>().Remove(entity);

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => await DbContext.Set<T>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public async Task<bool> IsExistAsync(Guid id, CancellationToken cancellationToken = default) => await DbContext.Set<T>().AnyAsync(e => e.Id == id, cancellationToken);

        public void Update(T entity) => DbContext.Set<T>().Update(entity);
    }
}
