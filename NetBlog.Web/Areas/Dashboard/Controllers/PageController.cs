using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetBlog.Repositories.Interfaces;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    [Authorize("Admin")]
    public class PageController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private INotyfService _notifyService;
        public PageController(IUnitOfWork unitOfWork, INotyfService notifyService)
        {
            _unitOfWork = unitOfWork;
            _notifyService = notifyService; 
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
