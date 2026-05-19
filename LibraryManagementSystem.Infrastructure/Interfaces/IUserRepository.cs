using LibraryManagementSystem.Domain.Entities;

namespace LibraryManagementSystem.Infrastructure.Interfaces
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsEmailExistsAsync(string email, CancellationToken cancellationToken = default);
    }
}
