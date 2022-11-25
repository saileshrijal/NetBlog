using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Repositories.Interfaces;
using NetBlog.ViewModels;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize(Roles ="Admin")]
    public class PageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private INotyfService _notifyService;
        public PageController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService; 
        }

        [HttpGet]
        public async Task<IActionResult> About()
        {
            var page = await _unitOfWork.Page.GetBy(x => x.Slug == "About");
            var vm = new PageViewModel();
            if (page != null)
            {
                vm = new PageViewModel(page);
            }
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> About(PageViewModel vm)
        {
            if (!ModelState.IsValid) { return View(vm); }
            var page = new PageViewModel().ConvertViewModel(vm);
            if (vm.Id == 0)
            {
                await _unitOfWork.Page.Create(page);
                await _unitOfWork.SaveAsync();
                _notifyService.Success("About page created successfully");
                return View();
            }
            page.Title = vm.Title;
            page.Description = vm.Description;
            page.ShortDescription = vm.ShortDescription;
            _unitOfWork.Page.Edit(page);
            await _unitOfWork.SaveAsync();
            _notifyService.Success("About page updated successfully");
            return View();
        }
    }
}
