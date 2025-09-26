using DataAccess.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.Models
{
    public class TimeLog : BaseEntity
    {
        [Required]
        public decimal Hours { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
