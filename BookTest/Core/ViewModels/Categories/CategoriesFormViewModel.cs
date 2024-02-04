using System.ComponentModel;

namespace Bookify.Web.Core.ViewModels.Categories
{
	public class CategoriesFormViewModel
	{
		public int Id { get; set; }
		
		[Remote("AllowItem", null, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		[DisplayName("Category")]
		public string Name { get; set; } = null!;

	}
}
