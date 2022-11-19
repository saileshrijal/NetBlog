using Microsoft.AspNetCore.Mvc;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
