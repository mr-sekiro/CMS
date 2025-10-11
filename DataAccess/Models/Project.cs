using DataAccess.Models.Shared;
using DataAccess.Models.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


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

        // --- FOREIGN KEYS ---
        public int ClientId { get; set; }
        public string TeamLeadId { get; set; } // string because it links to ApplicationUser's Id

        // --- NAVIGATION PROPERTIES ---
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("TeamLeadId")]
        public virtual ApplicationUser TeamLead { get; set; }

        // A project has many tasks
        public virtual ICollection<TaskItem> Tasks { get; set; } = new HashSet<TaskItem>();

        // A project has many team members (many-to-many)
        public virtual ICollection<ProjectTeamMember> TeamMembers { get; set; } = new HashSet<ProjectTeamMember>();
    }
}
