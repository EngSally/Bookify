namespace BookTest.Core.ViewModels.Users
{
	public class UserViewModel
	{
		public string Id { get; set; } = null!;
		public string Username { get; set; } = null!;
		public string FullName { get; set; } = null!;
		public string Email { get; set; } = null!;
		public bool Deleted { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? LastUpdateOn { get; set; }

	}
}
