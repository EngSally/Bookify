using BookTest.Core.ViewModels.BookCopy;

namespace BookTest.Core.ViewModels.Books
{
    public class BookDetailsViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Publisher { get; set; } = null!;
        public DateTime PublishingDate { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrlThumbnail { get; set; }
        public string Hall { get; set; } = null!;
        public bool IsAvailableForRental { get; set; }
        public string Description { get; set; } = null!;
        public IEnumerable<string> Categories { get; set; } = null!;
        public IEnumerable<BookCopyViewModel> BookCopies { get; set; } = null!;
        public bool Deleted { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
