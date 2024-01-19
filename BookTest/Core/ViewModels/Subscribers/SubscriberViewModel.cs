using Bookify.Web.Core.ViewModels.Rental;

namespace Bookify.Web.Core.ViewModels.Subscribers
{
	public class SubscriberViewModel
	{
		public int Id { get; set; }
		public string? Key { get; set; }

		public string FullName { set; get; } = null!;

		public string Email { get; set; } = null!;
		public int MobilNum { get; set; }

		public DateTime DateOfBirth { get; set; } = DateTime.Now;

		public string NationalId { get; set; } = null!;

		public string Address { get; set; } = null!;
		public string Area { get; set; } = null!;
		public string Governorate { get; set; } = null!;
		public DateTime CreatedOn { get; set; }
		public bool IsBlackListed { get; set; }
		public bool HasWhatsApp { get; set; }
		public string? ImageUrl { get; set; }

		public string? ImageUrlThumbnail { get; set; }

		public ICollection<RenewalSubscribtionViewModel> RenewalSubscribtions { get; set; } = new List<RenewalSubscribtionViewModel>();
		public ICollection<RentalViewModel> Rentals { get; set; } = new List<RentalViewModel>();

	}
}
