using System.ComponentModel;

namespace Bookify.Web.Core.ViewModels.Reports
{
	public class RentalsReportViewModel
	{
		[DisplayName("Duration")]
		[Required(ErrorMessage = Errors.Required)]
		public string Duration { get; set; } = null!;
		public PaginatedList<Bookify.Domain.Entities.RentalCopy> Rentals { get; set; }
	}
}
