using BookTest.Core.ViewModels.Books;
using BookTest.Core.ViewModels.Charts;
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
			throw new Exception("hhhhhhhhhhhhh");
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
            startDate ??= DateTime.Today.AddDays(-29);
            endDate  ??= DateTime.Today;

            var data=_context.RentalCopies
                                .Where(r=>r.RentalDate>=startDate &&r.RentalDate<endDate)
                                .GroupBy(r=>new {Date= r.RentalDate.Date })
                                .Select(  g=> new ChartItemViewModel()
                                {
                                    Label=g.Key.Date.ToString("d MMM"),
                                    Value=g.Count().ToString()
                                }).ToList();
            List<ChartItemViewModel> figures = new ();

            for (var day = startDate; day <= endDate; day = day.Value.AddDays(1))
            {
                var dayData = data.SingleOrDefault(d => d.Label == day.Value.ToString("d MMM"));

                ChartItemViewModel item = new()
                {
                    Label = day.Value.ToString("d MMM"),
                    Value = dayData is null ? "0" : dayData.Value
                };

                figures.Add(item);
            }
            return Ok(figures);
        }

        [AjaxOnly]
        public IActionResult GetSubscribersPerCity()
        {

            var data = _context.Subscribers
                .Include(s => s.Governorate)
                .Where(s => !s.Deleted)
                .GroupBy(s => new { GovernorateName = s.Governorate!.Name })
                .Select(g => new ChartItemViewModel
                {
                    Label = g.Key.GovernorateName,
                    Value = g.Count().ToString()
                })
                .ToList();

            return Ok(data);
        }

      
    }

}
