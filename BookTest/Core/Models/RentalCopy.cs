﻿namespace BookTest.Core.Models
{
	public class RentalCopy
	{
		public int RentalId { get; set; }
		public Rental? Rental { get; set; }
		public int BookCopyId { set; get; }
		public BookCopy? BookCopy { get; set; }
		public DateTime RentalDate { set; get; } = DateTime.Now;
		public DateTime EndDate { set; get; } = DateTime.Now.AddDays((int)RentalsConfigurations.RentalDuration);
		public DateTime? ReturnDate { set; get; }
		public DateTime? ExtendedOn { get; set; }
	}
}
