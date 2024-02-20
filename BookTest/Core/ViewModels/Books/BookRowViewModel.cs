namespace Bookify.Web.Core.ViewModels.Books
{
    public class BookRowViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Author { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateTime PublishingDate { get; set; }

        public string? ImageUrlThumbnail { get; set; }

        public string Hall { get; set; } = null!;

        public bool IsAvailableForRental { get; set; }

        public bool Deleted { get; set; }
    }
}
