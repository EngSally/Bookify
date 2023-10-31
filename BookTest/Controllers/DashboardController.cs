using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
