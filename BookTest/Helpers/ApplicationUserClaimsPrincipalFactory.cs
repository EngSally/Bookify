using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BookTest.Helpers
{
	public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
	{
		public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : base(userManager, roleManager, options)
		{
		}

		protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
		{
			var allClaimsIdentity = await base.GenerateClaimsAsync(user);
			allClaimsIdentity.AddClaim(new Claim("FullName", user.FullName));
			return allClaimsIdentity;
		}
	}
}
