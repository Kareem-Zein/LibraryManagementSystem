using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.DTOs.Service.Users
{
    public class CreateUser
    {
        public string FirstName { set; get; } = null!;
        public string LastName { set; get; } = null!;
        public string Email { set; get; } = null!;
        public string Password { set; get; } = null!;
        public UserType Type { set; get; }
    }
}
