using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace BookTest.Core.ViewModels.Subscribers
{
	public class SubscriberFormViewModel
	{
		public string? Key { get; set; }


		[MaxLength(100)]
		public string FristName { get; set; } = null!;
		[MaxLength(100)]
		public string LastName { get; set; } = null!;

		[AssertThat("DateOfBirth <= Today()", ErrorMessage = Errors.DateNotAtFuture)]
		public DateTime DateOfBirth { get; set; } = DateTime.Now;


		[MaxLength(15)]
		[RegularExpression(RegexPatterns.NationalId, ErrorMessage = Errors.InvalidNatialId)]
		[Remote("AllowNationalId", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string NationalId { get; set; } = null!;


		[MaxLength(20)]
		[RegularExpression(RegexPatterns.MobileNumber, ErrorMessage = Errors.InvalidUsername)]
		[Remote("AllowMobilNum", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string MobilNum { get; set; } = null!;
		public bool HasWhatsApp { get; set; }


		[MaxLength(150)]
		[EmailAddress]
		[Remote("AllowEmail", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string Email { get; set; } = null!;


		[MaxLength(500)]
		public string? ImageUrl { get; set; }


		[MaxLength(500)]
		public string? ImageUrlThumbnail { get; set; }

		[RequiredIf("Key == ''", ErrorMessage = Errors.EmptyImage)]
		public IFormFile? Image { get; set; }
		public string Address { get; set; } = null!;
		public bool IsBlackListed { get; set; }
		public int AreaId { get; set; }
		public IEnumerable<SelectListItem>? Areas { get; set; }
		public int GovernorateId { get; set; }
		public IEnumerable<SelectListItem>? Governorates { get; set; }

	}
}
