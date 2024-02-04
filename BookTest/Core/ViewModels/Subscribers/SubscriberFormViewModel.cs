using Microsoft.AspNetCore.Mvc.Rendering;
using UoN.ExpressiveAnnotations.NetCore.Attributes;

namespace Bookify.Web.Core.ViewModels.Subscribers
{
	public class SubscriberFormViewModel
	{
		public string? Key { get; set; }
        [Display(Name = "First Name")]
        public string FristName { get; set; } = null!;
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = null!;
        [Display(Name = "Date Of Birth")]
        [AssertThat("DateOfBirth <= Today()", ErrorMessage = Errors.DateNotAtFuture)]
		public DateTime DateOfBirth { get; set; } = DateTime.Now;
        [Display(Name = "National ID")]
        [Remote("AllowNationalId", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string NationalId { get; set; } = null!;
        [Display(Name = "Mobile Number")]
        [Remote("AllowMobilNum", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string MobilNum { get; set; } = null!;
		public bool HasWhatsApp { get; set; }
		[Remote("AllowEmail", null!, AdditionalFields = "Key", ErrorMessage = Errors.Duplicated)]
		public string Email { get; set; } = null!;
		[RequiredIf("Key == ''", ErrorMessage = Errors.EmptyImage)]
		public IFormFile? Image { get; set; }
		public string Address { get; set; } = null!;
		public bool IsBlackListed { get; set; }
		public int AreaId { get; set; }
		public IEnumerable<SelectListItem>? Areas { get; set; }
		public int GovernorateId { get; set; }
		public IEnumerable<SelectListItem>? Governorates { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageUrlThumbnail { get; set; }

    }
}
