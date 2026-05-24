using LibraryManagementSystem.Domain.Common;
using LibraryManagementSystem.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryManagementSystem.Domain.Entities
{
    public class User : EntityBase
    {
        [Column(TypeName = "nvarchar(100)")]
        public string FirstName { get; set; } = null!;
        
        [Column(TypeName = "nvarchar(100)")]
        public string LastName { get; set; } = null!;

        [Column(TypeName = "varchar(500)")]
        public string Email { set; get; } = null!;

        public UserType Type { set; get; }


        [Column(TypeName = "varchar(500)")]
        public string PasswordHash { set; get; } = null!;

        public bool IsActive { set; get; } = false;

        public List<RefreshToken> RefreshTokens { set; get; } = new List<RefreshToken>();
    }
}
