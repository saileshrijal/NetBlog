using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class PageController : BaseController
    {
        public PageController(UserManager<ApplicationUser> userManager, INotyfService notifyService, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService) : base(userManager, notifyService, userService, categoryService, pageService, webHostEnvironment, postService)
        {
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var vm = await _pageService.GetPage("About");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
            }
            if (vm.Id == 0)
            {
                vm.Slug = "About";
                await _pageService.CreatePage(vm);
                _notifyService.Success("About page created successfully");
                return RedirectToAction(nameof(About));
            }
            await _pageService.UpdatePage(vm);
            _notifyService.Success("About page updated successfully");
            return RedirectToAction(nameof(About));
        }


        [HttpGet]
        public async Task<IActionResult> Contact()
        {
            var vm = await _pageService.GetPage("Contact");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Contact(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
            }
            if (vm.Id == 0)
            {
                vm.Slug = "Contact";
                await _pageService.CreatePage(vm);
                _notifyService.Success("Contact page created successfully");
                return RedirectToAction(nameof(Contact));
            }
            await _pageService.UpdatePage(vm);
            _notifyService.Success("Contact page updated successfully");
            return RedirectToAction(nameof(Contact));
        }

        [HttpGet]
        public async Task<IActionResult> PrivacyPolicy()
        {
            var vm = await _pageService.GetPage("PrivacyPolicy");
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> PrivacyPolicy(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "page-img");
            }
            if (vm.Id == 0)
            {
                vm.Slug = "PrivacyPolicy";
                await _pageService.CreatePage(vm);
                _notifyService.Success("Privacy Policy page created successfully");
                return RedirectToAction(nameof(PrivacyPolicy));
            }
            await _pageService.UpdatePage(vm);
            _notifyService.Success("Privacy Policy page updated successfully");
            return RedirectToAction(nameof(PrivacyPolicy));
        }
    }
}
