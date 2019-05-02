using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WishList.Models;
using WishList.Models.AccountViewModels;

namespace WishList.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View("Register");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Register(RegisterViewModel user)
        {
            if(!ModelState.IsValid)
                return View("Register");

            var result = _userManager.CreateAsync(new ApplicationUser() { UserName = user.Email, Email = user.Email}, user.Password);

            if(!result.Result.Succeeded)
            {
                foreach(var error in result.Result.Errors )
                {
                    ModelState.AddModelError("Password", error.Description);
                }

                return View("Register", user);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
