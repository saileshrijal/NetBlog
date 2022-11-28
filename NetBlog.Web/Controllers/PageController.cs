using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Controllers
{
    public class PageController : Controller
    {
        private readonly IPostService _postService;
        private readonly IPageService _pageService;

        public PageController(IPostService postService, IPageService pageService)
        {
            _postService = postService;
            _pageService = pageService;
        }

        public async Task<IActionResult> About()
        {
            var vm = new BlogPageViewModel()
            {
                Page = await _pageService.GetPage("About"),
                RecentPosts = await _postService.GetRecentPosts()
            };
            return View(vm);
        }

        public async Task<IActionResult> Contact()
        {
            var vm = new BlogPageViewModel()
            {
                Page = await _pageService.GetPage("Contact"),
                RecentPosts = await _postService.GetRecentPosts()
            };
            return View(vm);
        }

        public async Task<IActionResult> PrivacyPolicy()
        {
            var vm = new BlogPageViewModel()
            {
                Page = await _pageService.GetPage("PrivacyPolicy"),
                RecentPosts = await _postService.GetRecentPosts()
            };
            return View(vm);
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
