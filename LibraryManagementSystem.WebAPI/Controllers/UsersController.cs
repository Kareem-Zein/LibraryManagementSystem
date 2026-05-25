using LibraryManagementSystem.Application.DTOs.Requests.Users;
using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService service)
        {
            _userService = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult<ResponseMessage<CreateUserResponse>>> CreateUser(CreateUserRequest data, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateAsync(data);

            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseMessage<LoginUserResponse>>> LoginUser(LoginUserRequest data, CancellationToken cancellationToken)
        {
            var response = await _userService.LoginAsync(data);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<ResponseMessage<LoginUserResponse>>> RefreshToken(string refreshToken, CancellationToken cancellationToken)
        {
            var response = await _userService.RefreshTokenAsync(refreshToken, cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<ActionResult<ResponseMessage<UserProfileResponse>>> Profile(CancellationToken cancellationToken)
        {
            var response = await _userService.GetProfile(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult<LogoutUserResponse>> Logout(CancellationToken cancellationToken)
        {
            var response = await _userService.LogoutAsync(HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier), cancellationToken);
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
