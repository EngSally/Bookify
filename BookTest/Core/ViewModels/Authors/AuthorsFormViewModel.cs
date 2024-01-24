namespace Bookify.Web.Core.ViewModels.Authors
{

	public class AuthorsFormViewModel
	{

		public int Id { get; set; }

		[Remote("Allow", null, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		
		public string Name { get; set; } = null!;
	}
}
