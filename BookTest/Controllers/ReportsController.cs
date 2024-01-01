using BookTest.Core.Models;
using BookTest.Core.ViewModels.Books;
using BookTest.Core.ViewModels.Rental;
using BookTest.Core.ViewModels.Reports;
using BookTest.Extensions;
using ClosedXML.Excel;
using ClosedXML.Excel.Drawings;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc.Rendering;
using OpenHtmlToPdf;
using System.IO;
using static System.Reflection.Metadata.BlobBuilder;

namespace BookTest.Controllers
{
    public class ReportsController : Controller
    {
       private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHost;
        private readonly IViewToHTMLService _viewToHtml;
        public ReportsController(ApplicationDbContext context, IMapper mapper, IWebHostEnvironment webHost, IViewToHTMLService viewToHtml)
        {
            _context = context;
            _mapper = mapper;
            _webHost = webHost;
            _viewToHtml = viewToHtml;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region BookReport
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
        public  async  Task<IActionResult>ExportBooksToExcel(string  authors, string categories)
        {
            var selectedAuthors=authors?.Split(',');
            var selectedCategories=categories?.Split(',');

           var books = _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Categories)
                        .ThenInclude(c => c.Category)
                        .Where(b => (string.IsNullOrEmpty(authors) || selectedAuthors!.Contains(b.AuthorId.ToString()))
                        && (string.IsNullOrEmpty(categories)|| b.Categories.Any(c => selectedCategories!.Contains(c.CategoryId.ToString()))))
                        .ToList();
         using  XLWorkbook workbook = new XLWorkbook();
            var sheet=workbook.AddWorksheet("Books");
             
            //sheet.Cell(1, 1).SetValue("Title");
            //sheet.Cell(1, 2).SetValue("Author");
            //sheet.Cell(1, 3).SetValue("Categories");
            //sheet.Cell(1, 4).SetValue("Publisher");
            //sheet.Cell(1, 5).SetValue("Publishing Date");
            //sheet.Cell(1, 6).SetValue("Hall");
            //sheet.Cell(1, 7).SetValue("Available for rental");
            //sheet.Cell(1, 8).SetValue("Status");
            var cellHeader=new string []{ "Title", "Author", "Categories", "Publisher", "Publishing Date", "Hall", "Available for rental", "Status"};
            sheet.AddHeader(cellHeader);

            for(int i = 0;i < books.Count; i++) 
            {
                sheet.Cell(i+2, 1).SetValue(books[i].Title);
                sheet.Cell(i+2, 2).SetValue(books[i].Author!.Name);
                sheet.Cell(i+2, 3).SetValue(string.Join(", ", (books[i].Categories!.Select(c => c.Category!.Name))));
                sheet.Cell(i+2, 4).SetValue(books[i].Publisher);
                sheet.Cell(i+2, 5).SetValue(books[i].PublishingDate.ToString("d MMM, yyyy"));
                sheet.Cell(i+2, 6).SetValue(books[i].Hall);
                sheet.Cell(i+2, 7).SetValue(books[i].IsAvailableForRental ? "Yes" : "No");
                sheet.Cell(i+2, 8).SetValue(books[i].Deleted ? "Deleted" : "Available");
            }
            sheet.Formate();
            await   using  var stream=new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", $"Books{DateTime.Today.Date}.xlsx");
        }

        public async Task<IActionResult> ExportBooksToPDF(string authors, string categories)
        {
            var selectedAuthors=authors?.Split(',');
            var selectedCategories=categories?.Split(',');

            var books = _context.Books
                        .Include(b => b.Author)
                        .Include(b => b.Categories)
                        .ThenInclude(c => c.Category)
                        .Where(b => (string.IsNullOrEmpty(authors) || selectedAuthors.Contains(b.AuthorId.ToString()))
                        && (string.IsNullOrEmpty(categories)|| b.Categories.Any(c => selectedCategories.Contains(c.CategoryId.ToString()))))
                        .ToList();
            // var html= await System.IO.File.ReadAllTextAsync($"{_webHost.WebRootPath}/templates/report.html");
            // html=    html.Replace("[Title]", "Books");
            // var body="<table><thead><tr><th>Title</th><th>Author</th></tr></thead><tbody>";
            //foreach(var book in books)
            // {
            //     body += $"<tr><td>{book.Title}</td><td>{book.Author!.Name}</td></tr>";
            // }
            // body += "</tbody></table>";
            // html=   html.Replace("[body]", body);

            var viewModel = _mapper.Map<IEnumerable<BookDetailsViewModel>>(books);

            var templatePath = "~/Views/Reports/BooksViewReport.cshtml";
            var html = await _viewToHtml.RenderViewToStringAsync(ControllerContext, templatePath, viewModel);
            var pdf = Pdf
                .From(html)
                .EncodedWith("Utf-8")
                .OfSize(PaperSize.A4)
                .WithMargins(1.Centimeters())
                .Landscape()
                .Content();

            return File(pdf.ToArray(), "application/octet-stream", $"Books{DateTime.Today.Date}.pdf");
        }
        #endregion

        #region   rental report

        public IActionResult Rentals(string duration, int? pageNumber)
        {
            var viewModel = new RentalsReportViewModel { Duration = duration };

            if (!string.IsNullOrEmpty(duration))
            {
                if (!DateTime.TryParse(duration.Split(" - ")[0], out DateTime from))
                {
                    ModelState.AddModelError("Duration", Errors.InvalidStartDate);
                    return View(viewModel);
                }

                if (!DateTime.TryParse(duration.Split(" - ")[1], out DateTime to))
                {
                    ModelState.AddModelError("Duration", Errors.InvalidEndDate);
                    return View(viewModel);
                }
                IQueryable<RentalCopy> rentals=
                 _context.RentalCopies
                 .Include(c => c.Rental)
                 .ThenInclude(r => r.Subscriber)
                 .Include(c => c.BookCopy)
                 .ThenInclude(b => b.Book)
                 .ThenInclude(b=>b.Author)
                 .Where(c => (c.RentalDate >= from && c.RentalDate <= to))
                 ;
                if (pageNumber is not null)
                    viewModel.Rentals = PaginatedList<RentalCopy>.Create(rentals, pageNumber ?? 0, (int)ReportsConfigurations.PageSize);
            }

            ModelState.Clear();

            return View(viewModel);
        }


        public async Task<IActionResult> ExportRentalsToExcel(string duration)
        {
                var from = DateTime.Parse(duration.Split(" - ")[0]);
                var to = DateTime.Parse(duration.Split(" - ")[1]);
                var rentals=
                 _context.RentalCopies
                 .Include(c => c.Rental)
                 .ThenInclude(r => r.Subscriber)
                 .Include(c => c.BookCopy)
                 .ThenInclude(b => b.Book)
                 .ThenInclude(b=>b.Author)
                 .Where(c => (c.RentalDate >= from && c.RentalDate <= to)).ToList()
                 ;
                using  XLWorkbook workbook = new XLWorkbook();
            var sheet=workbook.AddWorksheet("Rentals");

            var cellHeader=new string []{ "Subscriber ID", "Subscriber Name", "Subscriber Phone", "Book Title", "Book Author", "Book Serial", "Rental Date", "End Date","Return Date","Extended On"};
            sheet.AddHeader(cellHeader);

            for (int i = 0; i < rentals.Count; i++)
            {
                sheet.Cell(i + 2, 1).SetValue(rentals[i].Rental!.Subscriber!.Id );
                sheet.Cell(i + 2, 2).SetValue($"{rentals[i].Rental!.Subscriber!.FristName} {rentals[i].Rental!.Subscriber!.LastName}");
                sheet.Cell(i + 2, 3).SetValue(rentals[i].Rental!.Subscriber!.MobilNum);
                sheet.Cell(i + 2, 4).SetValue(rentals[i].BookCopy!.Book!.Title);
                sheet.Cell(i + 2, 5).SetValue(rentals[i].BookCopy!.Book!.Author!.Name);
                sheet.Cell(i + 2, 6).SetValue(rentals[i].BookCopy!.SerialNumber);
                sheet.Cell(i + 2, 7).SetValue(rentals[i].RentalDate.ToString("d MMM, yyyy"));
                sheet.Cell(i + 2, 8).SetValue(rentals[i].EndDate.ToString("d MMM, yyyy"));
                sheet.Cell(i + 2, 9).SetValue(rentals[i].ReturnDate?.ToString("d MMM, yyyy"));
                sheet.Cell(i + 2, 10).SetValue(rentals[i].ExtendedOn?.ToString("d MMM, yyyy"));
            }
            sheet.Formate();
            await   using  var stream=new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", $"Rentals{DateTime.Today.Date}.xlsx");
        }



        public async Task<IActionResult> ExportRentalsToPDF(string duration)
        {
            var from = DateTime.Parse(duration.Split(" - ")[0]);
            var to = DateTime.Parse(duration.Split(" - ")[1]);
            var rentals=
                 _context.RentalCopies
                 .Include(c => c.Rental)
                 .ThenInclude(r => r.Subscriber)
                 .Include(c => c.BookCopy)
                 .ThenInclude(b => b.Book)
                 .ThenInclude(b=>b.Author)
                 .Where(c => (c.RentalDate >= from && c.RentalDate <= to)).ToList()  ;
            var viewModel = _mapper.Map<IEnumerable<RentalCopy>>(rentals);

            var templatePath = "~/Views/Reports/RentalsViewReport.cshtml";
            var html = await _viewToHtml.RenderViewToStringAsync(ControllerContext, templatePath, viewModel);
            var pdf = Pdf
                .From(html)
                .EncodedWith("Utf-8")
                .OfSize(PaperSize.A4)
                .WithMargins(1.Centimeters())
                .Landscape()
                .Content();

            return File(pdf.ToArray(), "application/octet-stream", $"Rentals{DateTime.Today.Date}.pdf");
        }

        #endregion


        #region DelayedRentals
        public IActionResult DelayedRentals()
        {
            var rentals = _context.RentalCopies
                        .Include(c => c.BookCopy)
                        .ThenInclude(r => r!.Book)
                        .Include(c => c.Rental)
                        .ThenInclude(c => c!.Subscriber)
                        .Where(c => !c.ReturnDate.HasValue && c.EndDate < DateTime.Today)
                        .ToList();

            var viewModel = _mapper.Map<IEnumerable<RentalCopyViewModel>>(rentals);

            return View(viewModel);
        }


        public async Task<IActionResult> ExportDelayedRentalsToExcel()
        {
           var  delayedRentals =
                _context.RentalCopies
                        .Include(c => c.BookCopy)
                        .ThenInclude(r => r!.Book)
                        .Include(c => c.Rental)
                        .ThenInclude(c => c!.Subscriber)
                        .Where(c => !c.ReturnDate.HasValue && c.EndDate < DateTime.Today)
                        .ToList();
            var data = _mapper.Map<IList<RentalCopyViewModel>>(delayedRentals);
            using  XLWorkbook workbook = new XLWorkbook();
            var sheet=workbook.AddWorksheet("DelayedRentals");
            var imagepPath=$"{_webHost.WebRootPath}/assets/images/logo.png";
        //  sheet.AddPicture(imagepPath);

            var cellHeader=new string []{ "Subscriber ID", "Subscriber Name", "Subscriber Phone", "Book Title", "Book Serial", "Rental Date", "End Date","Extended On","Delay in Days"};
            sheet.AddHeader(cellHeader);

            for (int i = 0; i < delayedRentals.Count; i++)
            {
                sheet.Cell(i + 2, 1).SetValue(data[i].Rental!.Subscriber!.Id);
                sheet.Cell(i + 2, 2).SetValue(data[i].Rental!.Subscriber!.FullName);
                sheet.Cell(i + 2, 3).SetValue(data[i].Rental!.Subscriber!.MobilNum);
                sheet.Cell(i + 2, 4).SetValue(data[i].BookCopy!.BookTitle);
                sheet.Cell(i + 2, 5).SetValue(data[i].BookCopy!.SerialNumber);
                sheet.Cell(i + 2, 6).SetValue(data[i].RentalDate.ToString("d MMM, yyyy"));
                sheet.Cell(i + 2, 7).SetValue(data[i].EndDate.ToString("d MMM, yyyy"));
                 sheet.Cell(i + 2, 8).SetValue(data[i].ExtendedOn?.ToString("d MMM, yyyy"));
                 sheet.Cell(i + 2, 9).SetValue(data[i].DelayInDays);
            }
            sheet.Formate();
            await   using  var stream=new MemoryStream();
            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", $"DelayedRentals{DateTime.Today.Date}.xlsx");
        }



        public async Task<IActionResult> ExportDelayedRentalsToPDF()
        {
            var  delayedRentals =
                  _context.RentalCopies
                  .Include(c => c.Rental)
                  .ThenInclude(r => r.Subscriber)
                  .Include(c => c.BookCopy)
                  .ThenInclude(b => b.Book)
                  .Where(r => !r.ReturnDate.HasValue && r.EndDate < DateTime.Today).ToList();
            var viewModel = _mapper.Map<IList<RentalCopyViewModel>>(delayedRentals);
            var templatePath = "~/Views/Reports/DelayedRentalsViewReport.cshtml";
            var html = await _viewToHtml.RenderViewToStringAsync(ControllerContext, templatePath, viewModel);
            var pdf = Pdf
                .From(html)
                .EncodedWith("Utf-8")
                .OfSize(PaperSize.A4)
                .WithMargins(1.Centimeters())
                .Landscape()
                .Content();

            return File(pdf.ToArray(), "application/octet-stream", $"DelayedRentals{DateTime.Today.Date}.pdf");
        }


        #endregion



    }
}
