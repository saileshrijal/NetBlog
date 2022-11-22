using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class PostController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notifyService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostController(IWebHostEnvironment webHostEnvironment, ICategoryService categoryService, IUnitOfWork unitOfWork, INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _notifyService = notifyService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categoriesVM = await _categoryService.GetCategories();
            var postVM = new PostViewModel
            {
                Categories = categoriesVM.Select(x => new SelectListItem()
                {
                    Value = x.Id.ToString(),
                    Text = x.Title
                }).ToList()
            };
            return View(postVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var selectedCateogries = vm.Categories?.Where(x => x.Selected).Select(x=>x.Value).Select(int.Parse).ToList();
            var categories = selectedCateogries?.Select(x => new Category());
            var userId = _userManager.GetUserId(HttpContext.User);
            var post = new Post
            {
                Title = vm.Title,
                Description = vm.Description,
                UserId = userId,
                Slug = vm.Slug,
                ShortDescription = vm.ShortDescription,
                CreatedDate = vm.CreatedDate,
                IsBanner = vm.IsBanner,
                Status = vm.Status,
            };
            if(vm.Thumbnail != null)
            {
                post.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            if (vm.Title != null)
            {
                string slug = vm.Title.Trim();
                slug = slug.Replace(" ", "-");
                post.Slug = slug + "-" + Guid.NewGuid();
            }

            post.PostCategories = new List<PostCategory>();
            foreach (var categoryId in selectedCateogries)
            {
                post.PostCategories?.Add(new PostCategory
                {
                    Post = post,
                    CategoryId = categoryId,
                });
            }

            await _unitOfWork.Post.Create(post);
            await _unitOfWork.SaveAsync();
            _notifyService.Success("Post created successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
