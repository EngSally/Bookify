using BookTest.Core.ViewModels.Books;
using HashidsNet;
using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
	public class SearchController : Controller
	{
		private readonly IHashids _hashids;
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;

        public SearchController(IHashids hashids, ApplicationDbContext dbContext, IMapper mapper)
        {
            _hashids = hashids;
            _context = dbContext;
            _mapper = mapper;
        }

        public IActionResult Index()
		{
			return View();
		}
		

	public IActionResult Details( string bKey)
		{
            var bookId = _hashids.DecodeHex(bKey);

            if (bookId.Length == 0)
                return NotFound();

            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.BookCopies)
                .Include(b => b.Categories)
                .ThenInclude(c => c.Category)
                .SingleOrDefault(b => b.Id == int.Parse(bookId) && !b.Deleted);

            if (book is null)
                return NotFound();

            var viewModel = _mapper.Map<BookDetailsViewModel>(book);

            return View(viewModel);

        }


	}
}
