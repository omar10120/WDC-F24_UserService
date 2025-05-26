using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using WDC_F24.Domain.Consts;
using WDC_F24.Domain.Entities;

namespace WDC_F24.infrastructure.Repositories
{
    public static class Seeder
    {
        public static async Task SeedRolesAndAdminAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            var roles = new[] { Roles.Admin.ToString(), Roles.User.ToString() };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
            }

            var adminEmail = "admin@domain.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
              
                var admin = new User
                {
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "+20 123 123 123",
                    UserName = "Admin",
                    PhoneNumberConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                };
                await userManager.CreateAsync(admin, "Aa@112233!");
                await userManager.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
