﻿using BookTest.Core.Models;
using BookTest.Core.ViewModels.Reports;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
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
        public  async  Task<IActionResult>ExportBooksToExcel(string  authors, string categories)
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
         using  XLWorkbook workbook = new XLWorkbook();
            var sheet=workbook.AddWorksheet("Books");
             
            sheet.Cell(1, 1).SetValue("Title");
            sheet.Cell(1, 2).SetValue("Author");
            sheet.Cell(1, 3).SetValue("Categories");
            sheet.Cell(1, 4).SetValue("Publisher");
            sheet.Cell(1, 5).SetValue("Publishing Date");
            sheet.Cell(1, 6).SetValue("Hall");
            sheet.Cell(1, 7).SetValue("Available for rental");
            sheet.Cell(1, 8).SetValue("Status");
            var header=sheet.Range(1,1,1,8);
            header.Style.Fill.BackgroundColor = XLColor.Black;
            header.Style.Font.FontColor = XLColor.White;
            header.Style.Font.Bold = true;

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
            sheet.ColumnsUsed().AdjustToContents();
            sheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            sheet.CellsUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            sheet.Columns("A,C").Style.Fill.BackgroundColor = XLColor.RedRyb;

            sheet.CellsUsed().Style.Border.OutsideBorderColor = XLColor.Red;


            await   using  var stream=new MemoryStream();

            workbook.SaveAs(stream);
            return File(stream.ToArray(), "application/octet-stream", $"Books{DateTime.Today.Date}.xlsx");
        }

    }
}
