using Microsoft.AspNetCore.Identity;
using CalorieCounter.Core.Constants;
using CalorieCounter.Core.Models;
using System.Threading.Tasks;

namespace CalorieCounter.Repository.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(Roles.SUPERADMIN));
            await roleManager.CreateAsync(new IdentityRole(Roles.ADMIN));
            await roleManager.CreateAsync(new IdentityRole(Roles.DIETITIAN));
            await roleManager.CreateAsync(new IdentityRole(Roles.USER));
        }
    }
}