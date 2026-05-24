using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Application.DTOs.Requests.Users
{
    public class LoginUserRequest
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { set; get; } = null!;

        [StringLength(500, MinimumLength = 8, ErrorMessage = "Password length error")]
        public string Password { set; get; } = null!;
    }
}
