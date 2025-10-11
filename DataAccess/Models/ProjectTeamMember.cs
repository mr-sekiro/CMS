namespace DataAccess.Models
{
    // Join entity for the many-to-many relationship between Project and User (Team Members)
    public class ProjectTeamMember
    {
        public int ProjectId { get; set; }
        public virtual Project Project { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
