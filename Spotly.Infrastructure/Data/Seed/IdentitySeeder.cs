using Microsoft.AspNetCore.Identity;
using Spotly.Core.Constants;
using Spotly.Core.Entities;

namespace Spotly.Infrastructure.Data.Seed;

public static class IdentitySeeder
{
    public static async Task SeedAsync(RoleManager<IdentityRole<int>> roleManager, UserManager<AppUser> userManager,
        string adminEmail, string adminPassword)
    {
        foreach (var role in Roles.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        if (await userManager.FindByEmailAsync(adminEmail) is not null)
            return;

        var admin = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            FirstName = "Spotly",
            LastName = "Admin",
            CreatedAt = DateTime.UtcNow
        };

        var result = await userManager.CreateAsync(admin, adminPassword);
        if (!result.Succeeded)
            throw new InvalidOperationException(
                "Could not create the admin: " + string.Join("; ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(admin, Roles.Admin);
    }
}