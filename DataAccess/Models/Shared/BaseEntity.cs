using System.ComponentModel.DataAnnotations;
using System.Numerics;


namespace DataAccess.Models.Shared
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; }
    }
}
