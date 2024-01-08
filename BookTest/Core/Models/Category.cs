namespace BookTest.Core.Models
{
	[Index(nameof(Name), IsUnique = true)]
	public class Category : BaseModel
	{

		public int Id { get; set; }

		[MaxLength(100)]
		public String Name { get; set; } = null!;
		public ICollection<BookCategory> Books { get; set; } = new List<BookCategory>();
	}
}
