using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotdotdot.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(
            ILogger<HomeController> logger
        )
        {
            _logger = logger;
        }

        public IActionResult Index(string filename = null)
        {
            return View();
        }
    }
}
