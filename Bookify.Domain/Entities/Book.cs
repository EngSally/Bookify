namespace Bookify.Domain.Entities
{
	[Index(nameof(Title), nameof(AuthorId), IsUnique = true)]
	public class Book : BaseEntity
	{
		public int Id { get; set; }

		[MaxLength(500)]
		public string Title { get; set; } = null!;

		public int AuthorId { get; set; }
		public Author? Author { get; set; }

		[MaxLength(200)]
		public string Publisher { get; set; } = null!;

		public DateTime PublishingDate { get; set; }

		public string? ImageUrl { get; set; }
		public string? ImageUrlThumbnail { get; set; }
		public string? ImageUrlPublicId { get; set; }

		[MaxLength(50)]
		public string Hall { get; set; } = null!;

		public bool IsAvailableForRental { get; set; }

		public string Description { get; set; } = null!;

		public ICollection<BookCategory> Categories { get; set; } = new List<BookCategory>();
		public ICollection<BookCopy> BookCopies { get; set; } = new List<BookCopy>();

	}
}
