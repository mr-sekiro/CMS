using DataAccess.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public class TimeLog : BaseEntity
    {
        [Required]
        public decimal Hours { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }

        // --- FOREIGN KEYS ---
        public string UserId { get; set; }
        public int TaskItemId { get; set; }

        // --- NAVIGATION PROPERTIES ---
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("TaskItemId")]
        public virtual TaskItem TaskItem { get; set; }
    }
}
