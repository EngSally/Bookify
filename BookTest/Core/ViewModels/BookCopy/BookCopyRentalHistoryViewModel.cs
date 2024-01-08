namespace BookTest.Core.ViewModels.BookCopy
{
	public class BookCopyRentalHistoryViewModel
	{
		public string? SubscriberName { get; set; }
		public string? SubscriberNum { get; set; }

		public DateTime RentalDate { set; get; }
		public DateTime EndDate { set; get; }
		public DateTime? ReturnDate { set; get; }
		public DateTime? ExtendedOn { get; set; }
		public int DelayInDays
		{
			get
			{
				int delay=0;
				if (ReturnDate.HasValue && ReturnDate > EndDate)
				{
					delay = (ReturnDate.Value - EndDate).Days;

				}
				else if (!ReturnDate.HasValue && DateTime.Today > EndDate)
				{
					delay = (DateTime.Today - EndDate).Days;

				}
				return delay;
			}
		}

	}
}
