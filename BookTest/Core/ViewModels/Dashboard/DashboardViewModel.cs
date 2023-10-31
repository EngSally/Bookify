using BookTest.Core.ViewModels.Books;

namespace BookTest.Core.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public  int NumberOfCopies { get; set; }
        public int NumberOfSubscribers { set; get; }
        public IList<BookDetailsViewModel> LastAddedBooks { get; set; } = new List<BookDetailsViewModel>();
        public IList<BookDetailsViewModel> TopBooks { get; set; } = new List<BookDetailsViewModel>();
    }
}
