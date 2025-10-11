using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public string? EmailOtp { get; set; }
        public DateTime? OtpExpiryTime { get; set; }

        // --- NAVIGATION PROPERTIES ---

        // Projects where this user is the Team Lead
        public virtual ICollection<Project> LedProjects { get; set; }

        // Tasks assigned to this user
        public virtual ICollection<TaskItem> AssignedTasks { get; set; }

        // Time logs created by this user
        public virtual ICollection<TimeLog> TimeLogs { get; set; }

        // Projects this user is a team member of (many-to-many)
        public virtual ICollection<ProjectTeamMember> ProjectAssignments { get; set; }

        public ApplicationUser()
        {
            LedProjects = new HashSet<Project>();
            AssignedTasks = new HashSet<TaskItem>();
            TimeLogs = new HashSet<TimeLog>();
            ProjectAssignments = new HashSet<ProjectTeamMember>();
        }
    }
}
