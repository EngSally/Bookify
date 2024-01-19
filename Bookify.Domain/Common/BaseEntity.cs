using Bookify.Domain.Entities;

namespace Bookify.Domain.Common
{
	public class BaseEntity
	{
		public bool Deleted { get; set; }

		public string? CreatedById { get; set; }
		public ApplicationUser? CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public string? LastUpdateById { get; set; }
		public ApplicationUser? LastUpdateBy { get; set; }
		public DateTime? LastUpdateOn { get; set; }
	}
}
