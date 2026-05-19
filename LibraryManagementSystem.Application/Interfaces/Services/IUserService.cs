using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.DTOs.Service.Users;

namespace LibraryManagementSystem.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<ResponseMessage<UserResponse>> CreateAsync(CreateUser user, CancellationToken cancellationToken = default);
    }
}
