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
        private readonly IPostService _postService;

        public PostController(IPostService postService, IWebHostEnvironment webHostEnvironment, ICategoryService categoryService, IUnitOfWork unitOfWork, INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _notifyService = notifyService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
            _postService = postService;
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
            var userId = _userManager.GetUserId(HttpContext.User);
            vm.UserId = userId;
            if(vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            await _postService.CreatePost(vm);
            _notifyService.Success("Post created successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
