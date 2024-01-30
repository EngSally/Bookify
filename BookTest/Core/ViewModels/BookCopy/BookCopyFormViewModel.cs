namespace Bookify.Web.Core.ViewModels.BookCopy
{
	public class BookCopyFormViewModel
	{
		public int Id { get; set; }
		public int BookId { get; set; }

		[Display(Name = "Is Available For Rental?")]
		public bool IsAvailableForRental { get; set; }



		[Display(Name = "Edition Number")]
		//[Range(minimum: 1, maximum: 1000, ErrorMessage = Errors.RangNotBetween)]
		[Remote("AllowItem", null!, AdditionalFields = "BookId,EditionNumber", ErrorMessage = "Same Book Has Same EditionNumber")]
		public int EditionNumber { get; set; }

		public bool ShowRentalInput { get; set; }


	}
}
