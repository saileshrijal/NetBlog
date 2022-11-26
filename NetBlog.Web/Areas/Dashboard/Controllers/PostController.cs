using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetBlog.Models;
using NetBlog.Repositories.Interfaces;
using NetBlog.Services.Interfaces;
using NetBlog.Utilities;
using NetBlog.ViewModels;
using System.Drawing.Printing;
using X.PagedList;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize]
    public class PostController : BaseController
    {
        public PostController(UserManager<ApplicationUser> userManager, INotyfService notifyService, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService) : base(userManager, notifyService, userService, categoryService, pageService, webHostEnvironment, postService)
        {
        }

        public async Task<IActionResult> Index(int? page)
        {
            var loggedInUser = await GetLoggedInUser();
            var role = await GetRoleOfUser(loggedInUser);
            var vm = new List<PostViewModel>();
            if (role == "Admin")
            {
                vm = await _postService.GetPosts();
            }
            else
            {
                vm = await _postService.GetPostsByUserId(loggedInUser.Id);
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(await vm.ToPagedListAsync(pageNumber, pageSize));
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
            var loggedInUser = await GetLoggedInUser();
            vm.UserId = loggedInUser.Id;
            if(vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            await _postService.CreatePost(vm);
            _notifyService.Success("Post created successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var loggedInUser = await GetLoggedInUser();
            var role = await GetRoleOfUser(loggedInUser);
            var postVM = await _postService.GetPost(id);
            if (postVM.Id == 0) 
            {
                _notifyService.Error("Post not found");
                return RedirectToAction(nameof(Index)); 
            }
            if(loggedInUser.Id != postVM.UserId && role != "Admin")
            {
                _notifyService.Error("You are not authorized");
                return View(nameof(Index));
            }
            return View(postVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            if (vm.Thumbnail != null)
            {
                vm.ThumbnailUrl = FileHelper.UploadImage(vm.Thumbnail, _webHostEnvironment, "thumbnails");
            }
            await _postService.UpdatePost(vm);
            _notifyService.Success("Post updated successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var postVM = await _postService.GetPost(id);
            if (postVM.Id == 0)
            {
                _notifyService.Error("Post not found");
                return RedirectToAction(nameof(Index));
            }
            await _postService.DeletePost(id);
            _notifyService.Success("Post has been deleted successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
