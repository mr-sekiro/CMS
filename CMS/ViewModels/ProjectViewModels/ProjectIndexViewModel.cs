using DataAccess.Models.Shared.Enums;

namespace CMS.ViewModels.ProjectViewModels
{
    public class ProjectIndexViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string TeamLeadName { get; set; }
        public DateTime Deadline { get; set; }
        public ProjectStatus Status { get; set; }
    }
}
