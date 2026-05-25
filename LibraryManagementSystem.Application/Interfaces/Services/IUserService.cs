using LibraryManagementSystem.Application.DTOs.Requests.Users;
using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace LibraryManagementSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResponseMessage<CreateUserResponse>> CreateAsync(CreateUserRequest user, CancellationToken cancellationToken = default);
        Task<User?> GetByIdAsync(Guid guid, params Expression<Func<User, object>>[] includes);
        Task<ResponseMessage<UserProfileResponse>> GetProfile(Claim? claim, CancellationToken cancellationToken);
        Task<ResponseMessage<LoginUserResponse>> LoginAsync(LoginUserRequest user, CancellationToken cancellationToken = default);
        Task<ResponseMessage<LogoutUserResponse>> LogoutAsync(Claim? claim, CancellationToken cancellationToken);
        Task<ResponseMessage<LoginUserResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
