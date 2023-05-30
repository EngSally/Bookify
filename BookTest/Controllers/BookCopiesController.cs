using BookTest.Core.Models;
using BookTest.Core.ViewModels.BookCopy;
using BookTest.Core.ViewModels.Books;
using BookTest.Core.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
        [AutoValidateAntiforgeryToken]
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
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(BookCopyFormViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var bookCopy=_context.BooksCopies.Include(c=>c.Book).SingleOrDefault(c=>c.Id==model.Id);
            if (bookCopy == null) return NotFound();

            bookCopy = _mapper.Map(model, bookCopy);
            bookCopy.IsAvailableForRental = bookCopy.Book!.IsAvailableForRental ? model.IsAvailableForRental : false;
          
            bookCopy.LastUpdate = DateTime.Now;
            _context.SaveChanges();
            var bookCopyViewModel=_mapper.Map<BookCopyViewModel>(bookCopy);

            return PartialView("_BookCopyRow", bookCopyViewModel);


        }


        [HttpPost]
         [AutoValidateAntiforgeryToken]
        public IActionResult ToggleStatus(int id)
        {
            var copy = _context.BooksCopies.Find(id);

            if (copy is null)
                return NotFound();

            copy.Deleted = !copy.Deleted;
            copy.LastUpdate = DateTime.Now;

            _context.SaveChanges();

            return Ok();
        }

        public IActionResult AllowItem(BookCopyFormViewModel model)
        {
            var bookCopy = _context.BooksCopies.SingleOrDefault(b=>b.BookId==model.BookId&&b.EditionNumber==model.EditionNumber);
            bool allow=bookCopy is null||bookCopy.Id.Equals(model.Id);
            return Json(allow);
        }
    }
}
