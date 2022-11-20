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

        [HttpGet("NotFound")]
        public IActionResult NotFound()
        {
            return View();
        }
    }
}
