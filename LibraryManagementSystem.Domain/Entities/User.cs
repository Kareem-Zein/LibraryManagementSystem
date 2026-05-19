using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Domain.Entities
{
    public class User : EntityBase
    {
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First name length error")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First name can only contain letters")]
        public string FirstName { get; set; } = null!;
        
        [Column(TypeName = "nvarchar(100)")]
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last name length error")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last name can only contain letters")]
        public string LastName { get; set; } = null!;

        [Column(TypeName = "varchar(500)")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { set; get; } = null!;

        public UserType Type { set; get; }


        [Column(TypeName = "varchar(500)")]
        public string PasswordHash { set; get; } = null!;

        public bool IsActive { set; get; } = false;
    }
}
