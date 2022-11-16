using Microsoft.AspNetCore.Mvc;

namespace NetBlog.Web.Areas.Dashboard.Controllers
{
    [Area("Dashboard")]
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
