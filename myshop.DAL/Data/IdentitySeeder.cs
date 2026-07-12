using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using myshop.Models.Enums;

namespace myshop.DAL.Data
{
	public static class IdentitySeeder
	{
		public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
		{
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
			string[] roles = Enum.GetNames<UserTypeEnum>();
			foreach (var role in roles)
			{
				bool roleExist = await roleManager.RoleExistsAsync(role);
				if (!roleExist)
					await roleManager.CreateAsync(new IdentityRole<Guid>(role));
			}
		}
	}
}
