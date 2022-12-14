using Microsoft.AspNetCore.Identity;
using PohybStrava.Models;

namespace PohybStrava.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task RegisterAdmin(this WebApplication webApplication, string Email, string Password)
        {
            var adminRoleName = "Admin";
            using (var scope = webApplication.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

                if (!await roleManager.RoleExistsAsync(adminRoleName))
                    await roleManager.CreateAsync(new IdentityRole(adminRoleName));

               User user = await userManager.FindByEmailAsync(Email);

                if (user is null)
                    user = await CreateUser(userManager, Email, Password);

                if (!await userManager.IsInRoleAsync(user, adminRoleName))
                    await userManager.AddToRoleAsync(user, adminRoleName);
            }
        }

        private static async Task<User> CreateUser(UserManager<User> userManager, string Email, string Password)
        {
            User user = null;
            IdentityResult result = await userManager.CreateAsync(new User { UserName = Email, Email = Email }, Password);
            if (result.Succeeded)
            {
                user = await userManager.FindByEmailAsync(Email);
            }

            return user;
        }
    }
}
