using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Models;
using NetBlog.Services.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Dashboard")]
    public class CategoryController : BaseController
    {
        public CategoryController(UserManager<ApplicationUser> userManager, INotyfService notifyService, IUserService userService, ICategoryService categoryService, IPageService pageService, IWebHostEnvironment webHostEnvironment, IPostService postService) : base(userManager, notifyService, userService, categoryService, pageService, webHostEnvironment, postService)
        {
        }

        public async Task<IActionResult> Index()
        {
            var listOfCategories = await _categoryService.GetCategories();
            return View(listOfCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var loggedInUser = await GetLoggedInUser();
            vm.UserId = loggedInUser.Id;
            await _categoryService.CreateCategory(vm);
            _notifyService.Success("Category Created Successfully");
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategory(id);
            _notifyService.Success("Category Deleted Successfully");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var categorVM = await _categoryService.GetCategory(id);
            if (categorVM.Id == 0)
            {
                _notifyService.Error("Category Not Found");
                return RedirectToAction(nameof(Index));
            }
            return View(categorVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            await _categoryService.UpdateCategory(vm);
            _notifyService.Success("Category Updated Successfully");
            return RedirectToAction(nameof(Index));
        }
    }
}
