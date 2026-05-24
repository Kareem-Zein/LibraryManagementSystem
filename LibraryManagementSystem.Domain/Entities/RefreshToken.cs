using LibraryManagementSystem.Domain.Common;

namespace LibraryManagementSystem.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public string Token { get; set; } = null!;

        public bool IsRevoked { get; set; } = false;

        public DateTime? ExpireAtUTC { set; get; }

        public Guid UserId { set; get; }

        public User User { set; get; } = null!;
    }
}
