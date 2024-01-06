
using BookTest.Core.ViewModels.Books;
using BookTest.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using HashidsNet;

namespace BookTest.Controllers
{
    
    public class HomeController : Controller
    {
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ILogger<HomeController> _logger;
		private readonly IHashids _hashids;

		public HomeController(ApplicationDbContext context, IMapper mapper,  ILogger<HomeController> logger, IHashids hashids)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _hashids = hashids;
        }

        public IActionResult Index()
        {
           
            if (User.Identity!.IsAuthenticated)
				return RedirectToAction(nameof(Index), "Dashboard");
			var lastAddedBooks = _context.Books
									.Include(b => b.Author)
									.Where(b => !b.Deleted)
									.OrderByDescending(b => b.Id)
									.Take(10)
									.ToList();
			var viewModel=_mapper.Map<IEnumerable<BookDetailsViewModel>>(lastAddedBooks);
            foreach (var book in viewModel)
                book.Key = _hashids.EncodeHex(book.Id.ToString());
            return View(viewModel);
        }
      


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {ErrorCode= 404,ErrorDescription="Testttttttt" });
        }
    }
}