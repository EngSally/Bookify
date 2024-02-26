

using Bookify.Domain;

namespace Bookify.Infrastructure.Services
{
    public  interface IBooksService
    {
        public Book? GetById(int id);
        IQueryable<Book> GetDetails();
        (IQueryable<Book> books, int recordsTotal) GetFiltered(GetFilterDTO dto);
         Book Add(Book book, IEnumerable<int> selectedCategories, string createdById);
         Book Update(Book book, IEnumerable<int> selectedCategories, string updatedById);
        Book? GetWithCategories(int id);
        Book? ToggleStatus(int id, string updatedById);
        bool AllowTitle(int id, string title, int authorId);
        int GetActiveBooksCount();


    }
}
