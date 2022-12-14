using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService _postService;
        private readonly IPageService _pageService;

        public HomeController(IPostService postService, IPageService pageService)
        {
            _postService = postService;
            _pageService = pageService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var vm = new HomeViewModel()
            {
                RecentPosts = await _postService.GetRecentPosts(),
                BannerPosts = await _postService.GetBannerPosts()
            };
            return View(vm);
        }
    }
}