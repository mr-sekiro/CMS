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

        // --- FOREIGN KEYS ---
        public int ProjectId { get; set; }
        public string? AssignedToId { get; set; } // Nullable, as a task might be unassigned

        // --- NAVIGATION PROPERTIES ---
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("AssignedToId")]
        public virtual ApplicationUser AssignedTo { get; set; }

        // A task can have many time logs
        public virtual ICollection<TimeLog> TimeLogs { get; set; } = new HashSet<TimeLog>();
    }
}
