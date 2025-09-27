using CMS.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace CMS.Controllers
{
    public class AccountController : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult VerifyEmailOtp(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult ConfirmEmailSuccess()
        {
            return View();
        }
        [HttpGet]
        public IActionResult EmailSent()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ChangePassword(string Email, string Token)
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Token))
                return RedirectToAction("VerifyEmail", "Account");
            var model = new ChangePasswordViewModel
            {
                Email = Email,
                Token = Token
            };
            return View(model);
        }
    }
}
