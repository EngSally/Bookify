using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
    public class ReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Books() 
        {
            return View(); 
        }
    }
}
