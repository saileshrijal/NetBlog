using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;

namespace NetBlog.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostService _postService;

        public BlogController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index()
        {
            var listOfPostsVM = await _postService.GetPublishedPosts();
            return View(listOfPostsVM);
        }
    }
}
