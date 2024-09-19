using Events.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Events.Infrastructure.Data;

public class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new ApplicationRole(role));
                }
            }

            var users = new[]
            {
                new { UserName = "admin@gmail.com", Password = "Admin123!", Role = "Admin", Email="admin@gmail.com" },
                new { UserName = "user@gmail.com", Password = "User123!", Role = "User", Email="user@gmail.com"  }
            };

            foreach (var userInfo in users)
            {
                var user = await userManager.FindByNameAsync(userInfo.UserName);
                if (user == null)
                {
                    user = new ApplicationUser
                    {
                        UserName = userInfo.UserName,
                        Email = userInfo.Email,
                        EmailConfirmed = true,
                        LockoutEnabled = false
                    };
                    var result = await userManager.CreateAsync(user, userInfo.Password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, userInfo.Role);
                    }
                    else
                    {
                        throw new Exception($"Error occured on creating {userInfo.UserName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                    }
                }
            }
        }
    }
}
