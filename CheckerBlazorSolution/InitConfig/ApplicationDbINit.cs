using System;
using Microsoft.AspNetCore.Identity;

namespace CheckerBlazorServer.InitConfig;

public static class ApplicationDbInitializer
{
    public static void SeedUsers(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        if (userManager.FindByEmailAsync("abc@xyz.com").Result == null)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = "a@a.hu",
                Email = "a@a.hu"
            };


            IdentityResult result = userManager.CreateAsync(user, "Aa123456!").Result;

            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                roleManager.CreateAsync(new IdentityRole { Name = "Admin"}).GetAwaiter().GetResult();
            }

            if (result.Succeeded)
            {
                userManager.AddToRoleAsync(user, "Admin").Wait();
            }
        }
    }
}