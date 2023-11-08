using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
	public class SearchController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
