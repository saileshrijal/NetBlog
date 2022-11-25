using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetBlog.Web.Controllers
{
    [Area("Dashboard")]
    public class ErrorController : Controller
    {
        [HttpGet("AccessDenied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult NotFound()
        {
            return View();
        }
    }
}
