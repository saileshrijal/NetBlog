using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;

namespace NetBlog.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPageService _pageService;

        public HomeController(IPageService pageService)
        {
            _pageService = pageService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Page(string id)
        {
            var pageVM = await _pageService.GetPage(id);
            return View(pageVM);
        }
    }
}