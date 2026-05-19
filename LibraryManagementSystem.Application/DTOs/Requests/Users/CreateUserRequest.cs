using LibraryManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Application.DTOs.Requests.Users
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First name length error")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name length error")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { set; get; } = null!;

        public UserType Type { set; get; }


        [Column(TypeName = "varchar(500)")]
        [StringLength(500, MinimumLength = 8, ErrorMessage = "Password length error")]
        public string Password { set; get; } = null!;
    }
}
