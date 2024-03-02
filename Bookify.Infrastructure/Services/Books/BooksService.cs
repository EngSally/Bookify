


using Bookify.Domain;
using System.Linq.Dynamic.Core;
namespace Bookify.Infrastructure.Services
{
    internal class BooksService : IBooksService
    {
       private readonly ApplicationDbContext _context;
        public BooksService(ApplicationDbContext context)
        {
            _context = context;
        }


        public Book? GetById(int id)
        {
            return _context.Books.Find(id);
        }

        public  bool? BookAvailableForRental(int id)
        {

              return    _context!.Books.Where(b => b.Id == id).Select(b => new { b.IsAvailableForRental }).SingleOrDefault()?.IsAvailableForRental;
            



        }
        public IQueryable<Book> GetDetails()
        {
            
            return _context.Books
                   .Include(b => b.Author)
                   .Include(b => b.BookCopies)
                   .Include(b => b.Categories)
                   .ThenInclude(c => c.Category);

        }

        public  (IQueryable<Book> books, int recordsTotal) GetFiltered(GetFilterDTO dto)
        {
            IQueryable<Book> books   = GetDetails();
            if (!string.IsNullOrEmpty(dto.SearchValue))
                books = books.Where(b => b.Title.Contains(dto.SearchValue) || b.Author!.Name.Contains(dto.SearchValue));
            books = books
           .OrderBy($"{dto.ColSort} {dto.SortType}")
           .Skip(dto.Skip)
           .Take(dto.PageSize);
          
            var recordsTotal=books.Count();

            return (books,recordsTotal);

        }
        public Book Add(Book book, IEnumerable<int> selectedCategories, string createdById)
        {
            book.CreatedById = createdById;
                 foreach (var category in selectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });

            _context.Books.Add(book);
           _context.SaveChanges();

            return book;
        }

        public Book Update(Book book, IEnumerable<int> selectedCategories, string updatedById)
        {
            book.LastUpdateById = updatedById;
            book.LastUpdateOn = DateTime.Now;
            //book.ImageThumbnailUrl = GetThumbnailUrl(book.ImageUrl!);
            //book.ImagePublicId = imagePublicId;

            foreach (var category in selectedCategories)
                book.Categories.Add(new BookCategory { CategoryId = category });

            //.NET 6
            //if (!model.IsAvailableForRental)
            //    foreach (var copy in book.Copies)
            //        copy.IsAvailableForRental = false;
            _context.SaveChanges();

            //.NET 7
            //TODO //  BookCopies.SetAllAsNotAvailable
            //if (!book.IsAvailableForRental)
               // _context.BookCopies.SetAllAsNotAvailable(book.Id);

            return book;
        }
        public Book? GetWithCategories(int id)
        {
            return _context.Books.Include(b => b.Categories).SingleOrDefault(b=>b.Id==id);
        }
        public Book? ToggleStatus(int id, string updatedById)
        {
            var book = GetById(id);

            if (book is null)
                return null;
            book.Deleted = !book.Deleted;
            book.LastUpdateById = updatedById;
            book.LastUpdateOn = DateTime.Now;
            _context.SaveChanges();
            return book;
        }

        public bool AllowTitle(int id, string title, int authorId)
        {
            var book = _context.Books.FirstOrDefault(b => b.Title == title && b.AuthorId == authorId);
            return book is null || book.Id.Equals(id);
        }

        public int GetActiveBooksCount()
        {
            return _context.Books.Count(c => !c.Deleted);
        }

    }
}
