namespace Bookify.Web.Core.ViewModels.Users
{
	public class RestPasswordViewModel
	{
		public string Id { get; set; } = null!;

		[DataType(DataType.Password)]
		public string Password { get; set; } = null!;


		[DataType(DataType.Password),
			Display(Name = "Confirm password")]
		public string ConfirmPassword { get; set; } = null!;
	}
}
