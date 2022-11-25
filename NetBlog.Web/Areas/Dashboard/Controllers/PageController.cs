using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class PageController : Controller
    {
        private INotyfService _notifyService;
        private readonly IPageService _pageService;
        public PageController(INotyfService notifyService, IPageService pageService)
        {
            _notifyService = notifyService; 
            _pageService = pageService;
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
            if (vm.Id == 0)
            {
                vm.Slug = "About";
                await _pageService.CreatePage(vm);
                _notifyService.Success("About page created successfully");
                return View();
            }
            await _pageService.UpdatePage(vm);
            _notifyService.Success("About page updated successfully");
            return View();
        }
    }
}
