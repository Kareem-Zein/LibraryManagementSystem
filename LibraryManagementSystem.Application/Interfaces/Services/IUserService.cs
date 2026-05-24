using LibraryManagementSystem.Application.DTOs.Requests.Users;
using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;

namespace LibraryManagementSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResponseMessage<CreateUserResponse>> CreateAsync(CreateUserRequest user, CancellationToken cancellationToken = default);
        Task<ResponseMessage<LoginUserResponse>> LoginAsync(LoginUserRequest user, CancellationToken cancellationToken = default);
        Task<ResponseMessage<LoginUserResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
