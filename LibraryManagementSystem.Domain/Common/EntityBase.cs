using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Domain.Common
{
    public class EntityBase
    {
        [Key]
        public Guid Id { set; get; } = Guid.CreateVersion7();

        public DateTime CreatedAtUtc { set; get; } = DateTime.UtcNow;

        public DateTime? LastUpdatedAtUtc { set; get; } = null;
    }
}
