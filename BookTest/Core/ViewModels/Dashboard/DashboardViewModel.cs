using Bookify.Web.Core.ViewModels.Books;

namespace Bookify.Web.Core.ViewModels.Dashboard
{
	public class DashboardViewModel
	{
		public int NumberOfCopies { get; set; }
		public int NumberOfSubscribers { set; get; }
		public IEnumerable<BookDetailsViewModel> LastAddedBooks { get; set; } = new List<BookDetailsViewModel>();
		public IEnumerable<BookDetailsViewModel> TopBooks { get; set; } = new List<BookDetailsViewModel>();
	}
}
