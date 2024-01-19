namespace Bookify.Web.Core.ViewModels.Users
{
	public class RestPasswordViewModel
	{
		public string Id { get; set; } = null!;


		[MaxLength(100, ErrorMessage = Errors.MaxLength)]

		[DataType(DataType.Password),
		   StringLength(100, ErrorMessage = Errors.MaxMinLength,
		   MinimumLength = 8), RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
		public string Password { get; set; } = null!;


		[DataType(DataType.Password),
			Display(Name = "Confirm password"),
			Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
		public string ConfirmPassword { get; set; } = null!;
	}
}
