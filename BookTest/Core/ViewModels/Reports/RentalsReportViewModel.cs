using System.ComponentModel;

namespace BookTest.Core.ViewModels.Reports
{
	public class RentalsReportViewModel
	{
		[DisplayName("Duration")]
		[Required(ErrorMessage = Errors.Required)]
		public string Duration { get; set; } = null!;
		public PaginatedList<Models.RentalCopy> Rentals { get; set; }
	}
}
