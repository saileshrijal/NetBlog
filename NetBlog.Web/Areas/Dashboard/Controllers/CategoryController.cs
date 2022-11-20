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
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly INotyfService _notifyService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICategoryService categoryService, INotyfService notifyService, UserManager<ApplicationUser> userManager)
        {
            _categoryService = categoryService;
            _notifyService = notifyService;
            _userManager = userManager;
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
            var loggedInUserId = _userManager.GetUserId(HttpContext.User);
            vm.UserId = loggedInUserId;
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
            if(categorVM == null)
            {
                return NotFound();
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
