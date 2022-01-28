using FrontToBack.Models;
using FrontToBack.Utils;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Register

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var dbUser = await _userManager.FindByNameAsync(registerViewModel.UserName);
            if (dbUser != null)
            {
                ModelState.AddModelError("Username", "There is a user with this name!");
                return View();
            }

            var newUser = new AppUser
            {
                FullName = registerViewModel.FullName,
                UserName = registerViewModel.UserName,
                Email = registerViewModel.Email,
            };

            var identityResult = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _signInManager.SignInAsync(newUser, false);

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region Login

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var existUser = await _userManager.FindByEmailAsync(loginViewModel.Email);
            if (existUser == null)
            {
                ModelState.AddModelError("", "Email or password is invalid.");
                return View();
            }

            var loginResult = await _signInManager.PasswordSignInAsync(existUser, loginViewModel.Password,
                false, true);
            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Email or password is invalid.");
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        #endregion

        #region ForgotPassword

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            var dbUser = await _userManager.FindByEmailAsync(email);
            if (dbUser is null)
                return NotFound();

            //controller/action/id?token=asdasdas

            var token = await _userManager.GeneratePasswordResetTokenAsync(dbUser);

            var link = Url.Action("ResetPassword", "Account", new { dbUser.Id, token }, protocol: HttpContext.Request.Scheme);

            var message = $"<a href='{link}'>reset password</a>";

            await EmailUtil.SendEmailAsync(email, "Reset Password", message);

            return RedirectToAction("Login");
        }

        public async Task<IActionResult> ResetPassword(string id, string token)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return BadRequest();

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(string id, string token, ResetPasswordViewModel resetPasswordVm)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(token))
                return BadRequest();

            if (!ModelState.IsValid)
            {
                return View();
            }

            var dbUser = await _userManager.FindByIdAsync(id);
            if (dbUser is null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(dbUser, token, resetPasswordVm.NewPassword);
            if (result.Errors == null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction("Login");
        }

        #endregion
    }
}
