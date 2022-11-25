using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    public class BaseController : Controller
    {
        public UserManager<ApplicationUser> _userManager;
        public INotyfService _notifyService;
        public ICategoryService _categoryService;
        public IPageService _pageService;
        public IWebHostEnvironment _webHostEnvironment;
        public IPostService _postService;
        public IUserService _userService;
        public BaseController(UserManager<ApplicationUser> userManager,
                                INotyfService notifyService,
                                IUserService userService,
                                ICategoryService categoryService,
                                IPageService pageService,
                                IWebHostEnvironment webHostEnvironment,
                                IPostService postService)
        {
            _userManager = userManager;
            _notifyService = notifyService;
            _userService = userService;
            _categoryService = categoryService;
            _pageService = pageService;
            _webHostEnvironment = webHostEnvironment;
            _postService = postService;
        }

        public async Task<ApplicationUser> GetLoggedInUser()
        {
            return await _userManager.GetUserAsync(HttpContext.User);
        }

        public async Task<string> GetRoleOfUser(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return roles[0];
        }
    }
}
