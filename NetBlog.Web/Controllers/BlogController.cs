using Microsoft.AspNetCore.Mvc;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;
using X.PagedList;

namespace NetBlog.Web.Controllers
{
    public class BlogController : Controller
    {
        private readonly IPostService _postService;

        public BlogController(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<IActionResult> Index(int? page, string? search, string? category)
        {
            var listOfPostsVM = new List<PostViewModel>();
            if(search != null)
            {
                listOfPostsVM = await _postService.GetPublishedSearchPosts(search);
                ViewBag.SearchString = search;
            }
            else if (category != null)
            {
                listOfPostsVM = await _postService.GetPublishedPostsByCategory(category);
                ViewBag.CategoryString = category;
            }
            else 
            {
                listOfPostsVM = await _postService.GetPublishedPosts();
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await listOfPostsVM.ToPagedListAsync(pageNumber,pageSize));
        }


        public async Task<IActionResult> Post(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction(nameof(NotFound));
            }
            var vm = new BlogPostViewModel()
            {
                Post = await _postService.GetPublishedPost(id),
                RecentPosts = await _postService.GetRecentPosts()
            };
            if (vm.Post?.Id == 0)
            {
                return RedirectToAction(nameof(NotFound));
            }
            return View(vm);
        }
    }
}
