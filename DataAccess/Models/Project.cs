using DataAccess.Models.Shared;
using DataAccess.Models.Shared.Enums;
using System.ComponentModel.DataAnnotations;


namespace DataAccess.Models
{
    public class Project : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
