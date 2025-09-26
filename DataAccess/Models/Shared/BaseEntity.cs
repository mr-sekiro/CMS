using System.ComponentModel.DataAnnotations;


namespace DataAccess.Models.Shared
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateModified { get; set; } = DateTime.UtcNow;
    }
}
