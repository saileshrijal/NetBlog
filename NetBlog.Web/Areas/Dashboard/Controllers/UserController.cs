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
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public INotyfService _notifyService { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _webHost;

        public UserController(IUserService userService, 
            INotyfService notifyService, 
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment webHost)
        {
            _userService = userService;
            _notifyService = notifyService;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _webHost = webHost;
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
            return View();
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
            var loggedInUserId = _userManager.GetUserId(HttpContext.User);
            var profileVM = await _userService.GetUserProfileById(loggedInUserId);
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(ProfileViewModel vm) 
        {
            if (!ModelState.IsValid){ return View(vm); }
            var loggedInUserId = _userManager.GetUserId(HttpContext.User);
            var profileVM = await _userService.GetUserProfileById(loggedInUserId);
            if (vm.ProfilePicture != null)
            {
                profileVM.ProfilePictureUrl = FileHelper.UploadImage(vm.ProfilePicture, _webHost,"user-img");
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
