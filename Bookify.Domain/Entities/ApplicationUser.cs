

namespace Bookify.Domain.Entities
{
	
	public class ApplicationUser : IdentityUser
	{
		public string FullName { get; set; } = null!;
		public bool Deleted { get; set; }
		public string? CreatedById { get; set; }
		public DateTime CreatedOn { get; set; } 
		public string? LastUpdateById { get; set; }
		public DateTime? LastUpdateOn { get; set; }

	}
}
