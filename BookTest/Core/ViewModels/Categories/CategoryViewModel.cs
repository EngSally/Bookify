namespace Bookify.Web.Core.ViewModels.Categories
{
	public class CategoryViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public bool Deleted { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? LastUpdateOn { get; set; }
	}
}
