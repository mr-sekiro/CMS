using CMS.Services.EmailServices;
using CMS.ViewModels.AccountViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

namespace CMS.Controllers
{
    public class AccountController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager, IEmailService _emailService) : Controller
    {

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var address = new MailAddress(model.Email);
            var userName = address.User;
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                {

                    ModelState.AddModelError("", "Email or Password is incorrect");
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //extracting the username part
                string userName = model.Email;
                var address = new MailAddress(model.Email);
                userName = address.User;

                ApplicationUser user = new ApplicationUser
                {
                    FullName = model.Name,
                    Email = model.Email,
                    //UserName = model.Email.Split('@')[0]
                    UserName = userName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Developer");

                    var otp = new Random().Next(100000, 999999).ToString();
                    user.EmailOtp = otp;
                    user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(5); // valid for 5 mins
                    await _userManager.UpdateAsync(user);

                    // Send OTP via Email
                    await _emailService.SendEmailAsync(user.Email, "Email Confirmation OTP",
                        $"Your confirmation code is: <b>{otp}</b>. It will expire in 5 minutes.");

                    return RedirectToAction("VerifyEmailOtp", new { email = user.Email });
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }
            }
            return View(model);


        }
        [HttpGet]
        public IActionResult VerifyEmailOtp(string email)
        {
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmailOtp(string email, VerfiyEmailOTPViewModel model)
        {
            ViewBag.Email = email;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    if (user.EmailOtp == model.OTP && user.OtpExpiryTime > DateTime.UtcNow)
                    {
                        user.EmailConfirmed = true;
                        user.EmailOtp = null;
                        user.OtpExpiryTime = null;

                        await _userManager.UpdateAsync(user);

                        return RedirectToAction("ConfirmEmailSuccess");
                    }

                    ModelState.AddModelError("", "Invalid or expired OTP");
                    return View();
                }

                else
                {
                    ModelState.AddModelError("", "User not found!");
                    return View(model);
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult VerifyEmail()
        {
            return View();
        }
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetLink = Url.Action("ChangePassword", "Account", new { model.Email, Token = resetToken }, Request.Scheme);
                    var Subject = "Reset Password";
                    var Body = $"Please reset your password by clicking here: <a href='{resetLink}'>Reset Password</a>";
                    await _emailService.SendEmailAsync(model.Email, Subject, Body);
                    return RedirectToAction("EmailSent", "Account");
                }

                else
                {
                    ModelState.AddModelError("", "User not found!");
                    return View(model);
                }
            }
            return View(model);
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
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "User not found!");
                    return View(model);
                }
                var resetResult = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (!resetResult.Succeeded)
                {
                    foreach (var error in resetResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

