using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.DTOs.Response.Users
{
    public class UserResponse
    {
        public string FirstName { set; get; } = null!;
        public string LastName { set; get; } = null!;
        public string Email { set; get; } = null!;

        public UserType Type { set; get; }
    }
}
