using Microsoft.AspNetCore.Mvc;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    public class BaseController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
    }
}
