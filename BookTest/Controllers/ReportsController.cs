using BookTest.Core.ViewModels.Reports;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BookTest.Controllers
{
    public class ReportsController : Controller
    {
       private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public ReportsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Books(IList<int> selectedAuthors, IList<int> selectedCategories,
            int? pageNumber)
        {
            var authors = _context.Authors.OrderBy(a => a.Name).ToList();
            var categories = _context.Categories.OrderBy(a => a.Name).ToList();

            IQueryable<Book> books = _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Categories)
                        .ThenInclude(c => c.Category)
                        .Where(b => (!selectedAuthors.Any() || selectedAuthors.Contains(b.AuthorId))
                        && (!selectedCategories.Any() || b.Categories.Any(c => selectedCategories.Contains(c.CategoryId))));

            //if (selectedAuthors.Any())
            //    books = books.Where(b => selectedAuthors.Contains(b.AuthorId));

            //if (selectedCategories.Any())
            //    books = books.Where(b => b.Categories.Any(c => selectedCategories.Contains(c.CategoryId)));

            var viewModel = new BooksReportViewModel
            {
                Authors = _mapper.Map<IEnumerable<SelectListItem>>(authors),
                Categories = _mapper.Map<IEnumerable<SelectListItem>>(categories)
            };

            if (pageNumber is not null)
                viewModel.Books = PaginatedList<Book>.Create(books, pageNumber ?? 0, (int)ReportsConfigurations.PageSize);

            return View(viewModel);
        }

    }
}
