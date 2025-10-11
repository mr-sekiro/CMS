using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMS.ViewModels.AccountViewModels
{
    public class VerfiyEmailOTPViewModel
    {
        [Required(ErrorMessage = "OTP Code is required.")]
        [DisplayName("OTP CODE")]
        public string OTP { get; set; }
    }
}
