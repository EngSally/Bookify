using Microsoft.AspNetCore.Identity;

namespace BookTest.Seed
{
	public static class DefaultRoles
	{
		public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
		{
			if (!roleManager.Roles.Any())
			{
				await roleManager.CreateAsync(new IdentityRole(AppRole.Admin));
				await roleManager.CreateAsync(new IdentityRole(AppRole.Archive));
				await roleManager.CreateAsync(new IdentityRole(AppRole.Reception));

			}
		}
	}
}
