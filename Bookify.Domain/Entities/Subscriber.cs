namespace Bookify.Domain.Entities
{
	public class Subscriber : BaseEntity
	{
		public int Id { get; set; }
		public string FristName { get; set; } = null!;
		public string LastName { get; set; } = null!;
		public DateTime DateOfBirth { get; set; }
		public string NationalId { get; set; } = null!;
		public string MobilNum { get; set; } = null!;
		public bool HasWhatsApp { get; set; }
		public string Email { get; set; } = null!;
		public string? ImageUrl { get; set; }
		public string? ImageUrlThumbnail { get; set; }
		public string Address { get; set; } = null!;
		public bool IsBlackListed { get; set; }
		public int AreaId { get; set; }
		public Area? Area { get; set; }
		public int GovernorateId { get; set; }
		public Governorate? Governorate { get; set; }
		public ICollection<RenewalSubscribtion> RenewalSubscribtions { get; set; } = new List<RenewalSubscribtion>();
		public ICollection<Rental> Rentals { get; set; } = new List<Rental>();


	}
}
