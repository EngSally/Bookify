namespace BookTest.Core.ViewModels.BookCopy
{
    public class BookCopyViewModel
    {
        public int Id { get; set; }
        public string?  BookTitle { get; set; }
        public  int BookId { get;set; }
		public string? BookThumbnailURL { get; set; }
    	public bool IsAvailableForRental { get; set; }
        public int EditionNumber { get; set; }
        public int SerialNumber { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

    }
}
