using Bookify.Web.Core.ViewModels.BookCopy;

namespace Bookify.Web.Core.ViewModels.Rental
{
	public class RentalFormViewModel
	{
		public int? Id { get; set; }
		public string SubscriberKey { get; set; } = null!;
		public IList<int> SelectedCopies { get; set; } = new List<int>();
		public IEnumerable<BookCopyViewModel> CurrentCopies { get; set; } = new List<BookCopyViewModel>();
		public int? CountAvailableForRental { get; set; }
	}
}
