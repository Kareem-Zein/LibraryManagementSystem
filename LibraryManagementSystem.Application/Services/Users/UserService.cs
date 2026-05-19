using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.DTOs.Service.Users;
using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Application.Interfaces.Services;

namespace LibraryManagementSystem.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository repo)
        {
            _userRepo = repo;
        }

        public async Task<ResponseMessage<UserResponse>> CreateAsync(CreateUser user, CancellationToken cancellationToken = default)
        {
            if (await _userRepo.IsEmailExistsAsync(user.Email))
                return ResponseMessage<UserResponse>.Conflict("Email already exists");

            var newUser = new Domain.Entities.User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Type = user.Type
            };

            await _userRepo.AddAsync(newUser, cancellationToken);

            return ResponseMessage<UserResponse>.Created(new UserResponse
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Type = newUser.Type
            });
        }
    }
}
