using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Domain.Common
{
    public class EntityBase
    {
        [Key]
        public Guid Id { set; get; }

        public DateTime CreatedAt { set; get; }
        public DateTime LastUpdatedAt { set; get; }
    }
}
