using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels.Users
{
	public class UserFormViewModel
	{

		public string? Id { get; set; }
		[MaxLength(100, ErrorMessage = Errors.MaxLength), Display(Name = "Full Name"),
			RegularExpression(RegexPatterns.CharactersOnly_Eng, ErrorMessage = Errors.OnlyEnglishLetters)]
		public string FullName { get; set; } = null!;



		[MaxLength(100, ErrorMessage = Errors.MaxLength)]
		[Remote("AllowUserName", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		[RegularExpression(RegexPatterns.Username, ErrorMessage = Errors.InvalidUsername)]
		public string UserName { get; set; } = null!;
		[EmailAddress, MaxLength(100, ErrorMessage = Errors.MaxLength)]
		[Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		public string Email { get; set; } = null!;


		[MaxLength(100, ErrorMessage = Errors.MaxLength)]
		[DataType(DataType.Password),
		 StringLength(100, ErrorMessage = Errors.MaxMinLength,
			   MinimumLength = 8), RegularExpression(RegexPatterns.Password, ErrorMessage = Errors.WeakPassword)]
		[RequiredIf("Id==null", ErrorMessage = Errors.Required)]
		public string? Password { get; set; }


		[DataType(DataType.Password),
			Display(Name = "Confirm password"),
			Compare("Password", ErrorMessage = Errors.ConfirmPasswordNotMatch)]
		[RequiredIf("Id==null", ErrorMessage = Errors.Required)]
		public string? ConfirmPassword { get; set; }

		[DisplayName("Roles")]
		public IList<string> SelectedRoles { get; set; } = new List<string>();
		public IEnumerable<SelectListItem>? Roles { get; set; }

	}
}
