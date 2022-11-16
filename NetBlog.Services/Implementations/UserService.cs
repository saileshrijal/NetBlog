using Microsoft.AspNetCore.Identity;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task Login(LoginViewModel vm)
        {
            await _signInManager.PasswordSignInAsync(vm.UserName, vm.Password, vm.RememberMe, false);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
