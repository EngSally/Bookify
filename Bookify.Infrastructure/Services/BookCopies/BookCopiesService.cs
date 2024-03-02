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
    }
}
