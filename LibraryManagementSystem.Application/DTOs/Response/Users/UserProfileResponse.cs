using LibraryManagementSystem.Domain.Entities;
using LibraryManagementSystem.Domain.Enums;

namespace LibraryManagementSystem.Application.DTOs.Response.Users
{
    public class UserProfileResponse
    {
        public string Email { set; get; } = null!;
        public string FirstName { set; get; } = null!;
        public string LastName { set; get; } = null!;
        public DateTime RegisterDate { set; get; }
        public UserType Type { set; get; }

        public UserProfileResponse() { }
        public UserProfileResponse(User user)
        {
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
            RegisterDate = user.CreatedAtUtc;
            Type = user.Type;
        }
    }
}
