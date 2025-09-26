using DataAccess.Models.Shared;
using DataAccess.Models.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class TaskItem : BaseEntity
    {
        [Required]
        [StringLength(250)]
        public string Title { get; set; }
        public string Description { get; set; }
        public Shared.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
    }
}
