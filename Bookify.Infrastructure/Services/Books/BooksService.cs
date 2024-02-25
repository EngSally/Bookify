


namespace Bookify.Infrastructure.Services
{
    internal class BooksService : IBooksService
    {
       private readonly ApplicationDbContext _context;
        public BooksService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IQueryable<Book> GetDetails()
        {
            return _context.Books
                   .Include(b => b.Author)
                   .Include(b => b.BookCopies)
                   .Include(b => b.Categories)
                   .ThenInclude(c => c.Category);

             
        }
    }
}
