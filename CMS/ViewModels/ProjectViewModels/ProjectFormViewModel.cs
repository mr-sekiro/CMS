using DataAccess.Models.Shared.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels.ProjectViewModels
{
    public class ProjectFormViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today;

        [Required]
        public DateTime Deadline { get; set; } = DateTime.Today.AddMonths(1);

        [Required]
        public ProjectStatus Status { get; set; }

        [Required]
        [Display(Name = "Client")]
        public int ClientId { get; set; }

        [Required]
        [Display(Name = "Team Lead")]
        public string TeamLeadId { get; set; }

        // For populating the dropdowns
        public IEnumerable<SelectListItem> ClientList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> TeamLeadList { get; set; } = new List<SelectListItem>();
    }
}
