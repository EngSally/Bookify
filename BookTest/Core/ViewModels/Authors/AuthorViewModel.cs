namespace BookTest.Core.ViewModels.Authors
{
	public class AuthorViewModel
	{
		public int Id { get; set; }
		[MaxLength(100)]
		public string Name { get; set; } = null!;
		public bool Deleted { get; set; }
		public DateTime CretedOn { get; set; } = DateTime.Now;
		public DateTime? LastUpdate { get; set; }

	}
}
