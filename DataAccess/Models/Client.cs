using DataAccess.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class Client : BaseEntity
    {
        [Required]
        [StringLength(150)]
        public string CompanyName { get; set; }
        [StringLength(100)]
        public string ContactPerson { get; set; }
        [EmailAddress]
        public string Email { get; set; }


        // --- NAVIGATION PROPERTY ---
        // A client can have many projects
        public virtual ICollection<Project> Projects { get; set; } = new HashSet<Project>();
    }
}
