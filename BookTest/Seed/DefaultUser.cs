using Microsoft.AspNetCore.Identity;

namespace BookTest.Seed
{
	public static class DefaultUser
	{
		public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
		{
			ApplicationUser admin = new()
			{
				UserName = "admin",
				Email = "admin@bookify.com",
				FullName = "Admin",
				EmailConfirmed = true
			};

			var user = await userManager.FindByEmailAsync(admin.Email);

			if (user is null)
			{
				await userManager.CreateAsync(admin, "P@ssword123");
				await userManager.AddToRoleAsync(admin, AppRole.Admin);
			}
		}
	}
}
