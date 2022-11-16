using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Controllers
{
    public class UserManagement : Controller
    {
        private readonly IUserService _userService;
        public INotyfService _notifyService { get; }
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagement(IUserService userService, UserManager<ApplicationUser> userManager, INotyfService notifyService)
        {
            _userService = userService;
            _userManager = userManager;
            _notifyService = notifyService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                await _userService.Login(loginViewModel);
                return RedirectToAction(nameof(Index), "Post", new { area = "Dashboard" });
            }
            return View(loginViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home");
        }

    }
}
