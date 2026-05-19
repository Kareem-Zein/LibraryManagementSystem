using LibraryManagementSystem.Application.DTOs.Requests.Users;
using LibraryManagementSystem.Application.DTOs.Response;
using LibraryManagementSystem.Application.DTOs.Response.Users;
using LibraryManagementSystem.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

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

        [HttpPost]
        public async Task<ActionResult<ResponseMessage<UserResponse>>> CreateUser(CreateUserRequest data, CancellationToken cancellationToken)
        {
            var response = await _userService.CreateAsync(new Application.DTOs.Service.Users.CreateUser
            {
                Email = data.Email,
                FirstName = data.FirstName,
                LastName = data.LastName,
                Password  = data.Password,
                Type = data.Type
            });

            return StatusCode((int)response.StatusCode, response);
        }
    }
}
