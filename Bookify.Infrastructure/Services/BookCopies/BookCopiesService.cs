using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Services.BookCopies
{
    internal class BookCopiesService:IBookCopiesService
    {
        private readonly ApplicationDbContext _context;
        public BookCopiesService(ApplicationDbContext context)
        {
            _context = context;
        }

        public   BookCopy Add(BookCopy bookCopy)
        {
            _context.BooksCopies.Add(bookCopy);
            _context.SaveChanges();
            return bookCopy;
        }
      public (BookCopy? copy, bool? BookAvailableForRental) GetById(int id)
        {
          var result=   _context.BooksCopies.
                Include(c=>c.Book).
                Select(c=>new  {
                     c.Id,
                    c.BookId,
                    CopyIsAvailableForRental=c.IsAvailableForRental,
                    BookIsAvailableForRental=c.Book.IsAvailableForRental,
                    c.EditionNumber,
                    c.SerialNumber}).SingleOrDefault(c => c.Id == id);
            if(result == null)  return (null,null);
            BookCopy bookCopy = new BookCopy
            {
                Id= result.Id,
                BookId=result.BookId,
                IsAvailableForRental=result.CopyIsAvailableForRental,
                EditionNumber=result.EditionNumber,
                SerialNumber=result.SerialNumber
            };
            var bookIsAvailableForRental=result.BookIsAvailableForRental;
            return (copy: bookCopy, BookAvailableForRental: bookIsAvailableForRental);
            
        }

       

       public  int Update(BookCopy bookCopy)
        {
            _context.Update(bookCopy);
           return  _context.SaveChanges(); 
        }

     public    BookCopy? ToggleStatus(int id,string userId)
        {
            var bookCopy=_context.BooksCopies.Find(id);
            if(bookCopy is null)   return null;
            bookCopy.Deleted = !bookCopy.Deleted;
            bookCopy.LastUpdateOn = DateTime.Now;
            bookCopy.LastUpdateById= userId;
            _context.SaveChanges();
            return bookCopy;

        }
        public  bool  AllowItem(int bookId,int copyId, int editionNumber)
        {
            var bookCopy = _context.BooksCopies.SingleOrDefault(b=>b.BookId==bookId&&b.EditionNumber==editionNumber);
              return  bookCopy is null||bookCopy.Id.Equals(copyId);
        }

    }
}
