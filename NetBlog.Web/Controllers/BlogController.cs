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

        public async Task<IActionResult> Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return StatusCode(404);
            }
            var postVM = await _postService.GetPublishedPost(id);
            if (postVM == null)
            {
                return StatusCode(404);
            }
            return View(postVM);
        }
    }
}
