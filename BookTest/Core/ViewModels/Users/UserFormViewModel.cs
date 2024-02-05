using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels.Users
{
	public class UserFormViewModel
	{

		public string? Id { get; set; }
		[Display(Name = "Full Name")]
		public string FullName { get; set; } = null!;
		[Remote("AllowUserName", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		
		public string UserName { get; set; } = null!;

	
		[Remote("AllowEmail", null!, AdditionalFields = "Id", ErrorMessage = Errors.Duplicated)]
		public string Email { get; set; } = null!;


		
		[DataType(DataType.Password)]
		[RequiredIf("Id==null", ErrorMessage = Errors.Required)]
		public string? Password { get; set; }


		[DataType(DataType.Password),
			Display(Name = "Confirm password")]
		[RequiredIf("Id==null", ErrorMessage = Errors.Required)]
		public string? ConfirmPassword { get; set; }

		[DisplayName("Roles")]
		public IList<string> SelectedRoles { get; set; } = new List<string>();
		public IEnumerable<SelectListItem>? Roles { get; set; }

	}
}
