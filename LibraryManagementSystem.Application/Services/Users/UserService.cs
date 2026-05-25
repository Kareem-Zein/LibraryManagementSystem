using LibraryManagementSystem.Application.DTOs.Requests.Users;
using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Application.Interfaces.Services;
using LibraryManagementSystem.Domain.Entities;
using System.Linq.Expressions;
using System.Security.Claims;

namespace LibraryManagementSystem.Application.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public UserService(IUserRepository repo, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _userRepo = repo;
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<ResponseMessage<CreateUserResponse>> CreateAsync(CreateUserRequest user, CancellationToken cancellationToken = default)
        {
            if (await _userRepo.IsEmailExistsAsync(user.Email))
                return ResponseMessage<CreateUserResponse>.Conflict("Email already exists");

            var newUser = new Domain.Entities.User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.Password),
                Type = user.Type
            };

            await _userRepo.AddAsync(newUser, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResponseMessage<CreateUserResponse>.Created(new CreateUserResponse
            {
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                Email = newUser.Email,
                Type = newUser.Type
            });
        }

        public async Task<User?> GetByIdAsync(Guid guid, params Expression<Func<User, object>>[] includes) => await _userRepo.GetByIdAsync(guid, default, includes);

        public async Task<ResponseMessage<UserProfileResponse>> GetProfile(Claim? claim, CancellationToken cancellationToken)
        {
            if (claim is null)
                return ResponseMessage<UserProfileResponse>.Unauthorized();

            var userId = Guid.Parse(claim.Value);

            if (userId == Guid.Empty)
                return ResponseMessage<UserProfileResponse>.Unauthorized();

            var user = await _userRepo.GetByIdAsync(userId);

            if (user is null)
                return ResponseMessage<UserProfileResponse>.Unauthorized();

            return ResponseMessage<UserProfileResponse>.Success(new UserProfileResponse(user));
        }

        public async Task<ResponseMessage<LoginUserResponse>> LoginAsync(LoginUserRequest user, CancellationToken cancellationToken = default)
        {
            var existingUser = await _userRepo.GetByEmailAsync(user.Email, cancellationToken);

            if (existingUser is null)
                return ResponseMessage<LoginUserResponse>.Unauthorized("Invalid email or password.");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, existingUser.PasswordHash))
                return ResponseMessage<LoginUserResponse>.Unauthorized("Invalid email or password.");

            var accessToken = _tokenService.GenerateAccessToken(existingUser);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var result = await _unitOfWork.ExecuteInTransactionAsync(async Task<bool> (ct) =>
            {
                _tokenService.RevokeAllUserRefreshTokens(existingUser.Id);
                await _tokenService.SaveRefreshToken(new Domain.Entities.RefreshToken
                {
                    UserId = existingUser.Id,
                    Token = refreshToken,
                    Id = Guid.CreateVersion7(),
                    ExpireAtUTC = DateTime.UtcNow.AddDays(30)
                });

                return true;
            }, cancellationToken);

            if (!result)
                return ResponseMessage<LoginUserResponse>.InternalError();

            return new ResponseMessage<LoginUserResponse>
            {
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,
                Message = "Login successful",
                Data = new LoginUserResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken
                }
            };
        }

        public async Task<ResponseMessage<LogoutUserResponse>> LogoutAsync(Claim? claim, CancellationToken cancellationToken)
        {
            if (claim is null)
                return ResponseMessage<LogoutUserResponse>.Unauthorized();

            var userId = Guid.Parse(claim.Value);

            if (userId == Guid.Empty)
                return ResponseMessage<LogoutUserResponse>.Unauthorized();

            _tokenService.RevokeAllUserRefreshTokens(userId);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResponseMessage<LogoutUserResponse>.Success(new LogoutUserResponse(), "Logout successful");
        }

        public async Task<ResponseMessage<LoginUserResponse>> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            return await _tokenService.RefreshToken(refreshToken, cancellationToken);
        }
    }
}
