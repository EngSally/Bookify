using BookTest.Core.ViewModels.Books;
using BookTest.Core.ViewModels.Dashboard;
using Microsoft.AspNetCore.Mvc;

namespace BookTest.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public DashboardController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public IActionResult Index()
        {
            var numberOfCopies = _context.BooksCopies.Count(c => !c.Deleted);
            numberOfCopies = numberOfCopies <= 10 ? numberOfCopies : numberOfCopies / 10 * 10;

            var numberOfsubscribers = _context.Subscribers.Count(c => !c.Deleted);
            var lastAddedBooks = _context.Books
                                .Include(b => b.Author)
                                .Where(b => !b.Deleted)
                                .OrderByDescending(b => b.Id)
                                .Take(8)
                                .ToList();

            var topBooks = _context.RentalCopies
                .Include(c => c.BookCopy)
                .ThenInclude(c => c!.Book)
                .ThenInclude(b => b!.Author)
                .GroupBy(c => new
                {
                    c.BookCopy!.BookId,
                    c.BookCopy!.Book!.Title,
                    c.BookCopy!.Book!.ImageUrlThumbnail,
                    AuthorName = c.BookCopy!.Book!.Author!.Name
                })
                .Select(b => new
                {
                    b.Key.BookId,
                    b.Key.Title,
                    b.Key.ImageUrlThumbnail,
                    b.Key.AuthorName,
                    Count = b.Count()
                })
                .OrderByDescending(b => b.Count)
                .Take(6)
                .Select(b => new BookDetailsViewModel
				{
                    Id = b.BookId,
                    Title = b.Title,
                    ImageUrlThumbnail = b.ImageUrlThumbnail,
                    Author = b.AuthorName
                })
                .ToList();

            var viewModel = new DashboardViewModel
            {
				NumberOfCopies = numberOfCopies,
				NumberOfSubscribers = numberOfsubscribers,
				LastAddedBooks = _mapper.Map<IEnumerable<BookDetailsViewModel>>(lastAddedBooks),
				TopBooks = topBooks

			};

            return View(viewModel);
           
        }

        [AjaxOnly]
        public IActionResult GetRentalsPerDay(DateTime? startDate, DateTime? endDate)
        {
            

            return Ok();
        }

        [AjaxOnly]
        public IActionResult GetSubscribersPerCity()
        {
            
            return Ok();
        }
    }

}
