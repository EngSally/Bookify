
using BookTest.Core.ViewModels.BookCopy;


namespace BookTest.Controllers
{
    public class BookCopiesController : Controller
    {
        private readonly   ApplicationDbContext  _context;
        private readonly IMapper _mapper;
       public BookCopiesController(ApplicationDbContext context,IMapper mapper)
        {
            _context=context;
            _mapper=mapper;
        }
        [AjaxOnly]
       
        public  IActionResult Create(int bookId)
        {
            var book=_context.Books.Find(bookId);
            if (book is null) return NotFound();

            var model=new BookCopyFormViewModel{
                BookId=bookId,
                ShowRentalInput=book.IsAvailableForRental
            };
            return PartialView("_FormBookCopy", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(BookCopyFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var book=_context.Books.Find(model.BookId);
            if (book is null) return NotFound();
            var bookCopy=_mapper.Map<BookCopy>(model);
            bookCopy.IsAvailableForRental = book.IsAvailableForRental ? model.IsAvailableForRental : false;
            _context.BooksCopies.Add(bookCopy);
            _context.SaveChanges();
            var bookCopyViewModel=_mapper.Map<BookCopyViewModel>(bookCopy);

            return PartialView("_BookCopyRow", bookCopyViewModel);
        }



        [AjaxOnly]
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var bookCopy=_context.BooksCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id==id);
            if(bookCopy is null) return NotFound();
            var model=_mapper.Map<BookCopyFormViewModel>(bookCopy);
            model.ShowRentalInput = bookCopy.Book!.IsAvailableForRental;
           
           return PartialView("_FormBookCopy", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookCopyFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var bookCopy=_context.BooksCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id==model.Id);
            if (bookCopy == null) return NotFound();

            bookCopy = _mapper.Map(model, bookCopy);
            bookCopy.IsAvailableForRental = bookCopy.Book!.IsAvailableForRental ? model.IsAvailableForRental : false;
          
            bookCopy.LastUpdateOn = DateTime.Now;
            _context.SaveChanges();
            var bookCopyViewModel=_mapper.Map<BookCopyViewModel>(bookCopy);

            return PartialView("_BookCopyRow", bookCopyViewModel);


        }
        public IActionResult RentalHistory(int id)
        {
            var rentals=_context.RentalCopies
                .Include(r=>r.Rental)
                .ThenInclude(r=>r.Subscriber)
                .Where(c=>c.BookCopyId==id).ToList();
            var model=_mapper.Map<IEnumerable<BookCopyRentalHistoryViewModel>>(rentals);
            return View(model);
        }

        [HttpPost]
         [ValidateAntiForgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var copy = _context.BooksCopies.Find(id);

            if (copy is null)
                return NotFound();

            copy.Deleted = !copy.Deleted;
            copy.LastUpdateOn = DateTime.Now;

            _context.SaveChanges();

            return Ok();
        }

        public IActionResult AllowItem(BookCopyFormViewModel model)
        {
            var bookCopy = _context.BooksCopies.SingleOrDefault(b=>b.BookId==model.BookId&&b.EditionNumber==model.EditionNumber);
            bool allow=bookCopy is null||bookCopy.Id.Equals(model.Id);
            return Json(allow);
        }

        public bool AreOccurrencesEqual(string s)
        {
            Dictionary<char,int> dic=new Dictionary<char, int> ();
            foreach(char c in s) 
            { 
             if(! dic.TryAdd(c,1))
                {
                    dic[c]++;
                }
            }
           
            for(int i=1; i < dic.Count; i++) 
            {
               if( dic.ElementAt(i-1).Value!=dic.ElementAt(i).Value)
                    return false;
            }
            return true;

        }
    }
}
