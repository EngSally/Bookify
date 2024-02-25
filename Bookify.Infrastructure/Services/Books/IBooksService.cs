

namespace Bookify.Infrastructure.Services
{
    public  interface IBooksService
    {
        IQueryable<Book> GetDetails();
    }
}
