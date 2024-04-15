

namespace Bookify.Infrastructure.Services
{
    public  interface IBookCopiesService
    
    {
        BookCopy Add(BookCopy bookCopy);
        (BookCopy?copy,bool? BookAvailableForRental)  GetById(int id);
       

        int Update(BookCopy bookCopy);
        BookCopy? ToggleStatus(int id, string userId);
        bool AllowItem(int bookId, int copyId, int editionNumber);


    }
}
