using AspNetCoreHero.ToastNotification.Abstractions;
using AspNetCoreHero.ToastNotification.Notyf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Authorize]
    [Area("Dashboard")]
    public class UserController : BaseController
    {
        public UserController(UserManager<ApplicationUser> userManager, INotyfService notifyService, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService) : base(userManager, notifyService, userService, categoryService, pageService, webHostEnvironment, postService)
        {
        }

        [Authorize(Roles = "Admin")]
        public async  Task<IActionResult> Index()
        {
            var ListOfUsersVm = await _userService.GetUsers();
            return View(ListOfUsersVm);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var existingUserByUserName = await _userManager.FindByNameAsync(vm.UserName);
            if (existingUserByUserName != null) 
            {
                _notifyService.Error($"Username: {vm.UserName} already exist");
                return View(vm);
            }
            var existingUserByEmail = await _userManager.FindByEmailAsync(vm.Email);
            if (existingUserByEmail != null) 
            {
                _notifyService.Error($"Email: {vm.Email} already exist");
                return View(vm);
            }
            await _userService.Register(vm);
            _notifyService.Success("User Created Successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> changeStatus(string id)
        {
            await _userService.ChangeStatus(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(Index), "Post", new { area = "Dashboard" });
            }
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var existingUser = await _userManager.FindByNameAsync(vm.UserName);
            if (existingUser == null)
            {
                _notifyService.Error("Incorrect Username");
                return View(vm);
            }
            if (!existingUser.Status)
            {
                _notifyService.Error("You are currently inactive. Please contact to admin");
                return View(vm);
            }
            var checkPassword = await _userManager.CheckPasswordAsync(existingUser, vm.Password);
            if (!checkPassword)
            {
                _notifyService.Error("Incorrect Password");
                return View(vm);
            }
            await _userService.Login(vm);
            return RedirectToAction(nameof(Index), "Post", new { area = "Dashboard" });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return RedirectToAction("Index", "Home",new {area=""});
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var loggedInUser = await GetLoggedInUser();
            var profileVM = await _userService.GetUserProfileById(loggedInUser.Id);
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel vm) 
        {
            if (!ModelState.IsValid){ return View(vm); }
            var loggedInUser = await GetLoggedInUser();
            var profileVM = await _userService.GetUserProfileById(loggedInUser.Id);
            if (vm.ProfilePicture != null)
            {
                profileVM.ProfilePictureUrl = FileHelper.UploadImage(vm.ProfilePicture, _webHostEnvironment,"user-img");
            }
            else
            {
                vm.ProfilePictureUrl = profileVM.ProfilePictureUrl;
            }
            await _userService.UpdateProfile(profileVM);
            _notifyService.Success("Profile updated successfully");
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                _notifyService.Error("User Not Found");
                return RedirectToAction(nameof(Index));
            }
            var vm = new ResetPasswordViewModel() {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
            };
            return View(vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            await _userService.ResetPassword(vm);
            _notifyService.Success("Password reset successfully");
            return RedirectToAction(nameof(Index));
        }

    }
}
