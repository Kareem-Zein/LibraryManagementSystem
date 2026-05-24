using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.Interfaces;
using LibraryManagementSystem.Application.Interfaces.Repositories;
using LibraryManagementSystem.Application.Interfaces.Services;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LibraryManagementSystem.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUnitOfWork _unitOfWork;
        public TokenService(IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _refreshTokenRepo = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public string GenerateAccessToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Type.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]));
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public async Task<ResponseMessage<LoginUserResponse>> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            try
            {
                var token = await _refreshTokenRepo.GetByTokenAsync(refreshToken, cancellationToken);

                if (token is null || token.IsRevoked || token.ExpireAtUTC <= DateTime.UtcNow)
                    return ResponseMessage<LoginUserResponse>.Unauthorized("Invalid refresh token");

                token.IsRevoked = true;

                var newRefreshToken = GenerateRefreshToken();
                var newAccessToken = GenerateAccessToken(token.User);

                var nRT = new RefreshToken()
                {
                    UserId = token.UserId,
                    Token = newRefreshToken,
                    ExpireAtUTC = DateTime.UtcNow.AddDays(30),
                    Id = Guid.CreateVersion7()
                };

                var result = await _unitOfWork.ExecuteInTransactionAsync(async Task<bool> (ct) =>
                {
                    try
                    {
                        await _refreshTokenRepo.AddAsync(nRT);
                        _refreshTokenRepo.Update(token);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }, cancellationToken);

                if (result)
                    return ResponseMessage<LoginUserResponse>.Success(new LoginUserResponse
                    {
                        RefreshToken = newRefreshToken,
                        Token = newAccessToken
                    });

                return ResponseMessage<LoginUserResponse>.InternalError();
            }
            catch
            {
                return ResponseMessage<LoginUserResponse>.InternalError();
            }
        }

        public void RevokeAllUserRefreshTokens(Guid userId)
        {
            _refreshTokenRepo.RevokeAllUserRefreshTokens(userId);
        }

        public async Task SaveRefreshToken(RefreshToken refreshToken) => await _refreshTokenRepo.AddAsync(refreshToken);
    }
}
