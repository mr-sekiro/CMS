using DataAccess.Models;
using DataAccess.Models.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                // Apply any pending migrations
                context.Database.Migrate();

                //Seed Roles
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("TeamLead"));
                    await roleManager.CreateAsync(new IdentityRole("Developer"));
                }

                //Seed Users
                if (!await userManager.Users.AnyAsync())
                {
                    // Create Admin User
                    var adminUser = new ApplicationUser
                    {
                        FullName = "Abdullah Hussein",
                        UserName = "admin@test.com",
                        Email = "admin@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(adminUser, "Password123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // Create Team Lead User
                    var teamLeadUser = new ApplicationUser
                    {
                        FullName = "Mohammed Salim",
                        UserName = "teamlead@test.com",
                        Email = "teamlead@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(teamLeadUser, "Password123!");
                    await userManager.AddToRoleAsync(teamLeadUser, "TeamLead");

                    // Create Developer User
                    var devUser = new ApplicationUser
                    {
                        FullName = "Ahmed Osman",
                        UserName = "developer@test.com",
                        Email = "developer@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(devUser, "Password123!");
                    await userManager.AddToRoleAsync(devUser, "Developer");
                }
            }
        }
    }
}
