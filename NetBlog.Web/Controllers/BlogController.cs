using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

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
            var vm = new BlogPostViewModel()
            {
                Post = await _postService.GetPublishedPost(id),
                RecentPosts = await _postService.GetRecentPosts()
            };
            if (vm.Post == null)
            {
                return StatusCode(404);
            }
            return View(vm);
        }
    }
}
