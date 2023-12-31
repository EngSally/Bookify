using System.ComponentModel;

namespace BookTest.Core.ViewModels.Reports
{
    public class DelayedRentalsReportViewModel
    {
        public PaginatedList<Models.RentalCopy> Rentals { get; set; }
    }
}
