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
                        UserName = "admin",
                        Email = "admin@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(adminUser, "Password123!");
                    await userManager.AddToRoleAsync(adminUser, "Admin");

                    // Create Team Lead User
                    var teamLeadUser = new ApplicationUser
                    {
                        FullName = "Mohammed Salim",
                        UserName = "teamlead",
                        Email = "teamlead@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(teamLeadUser, "Password123!");
                    await userManager.AddToRoleAsync(teamLeadUser, "TeamLead");

                    // Create Developer User
                    var devUser = new ApplicationUser
                    {
                        FullName = "Ahmed Osman",
                        UserName = "developer",
                        Email = "developer@test.com",
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(devUser, "Password123!");
                    await userManager.AddToRoleAsync(devUser, "Developer");
                }
                // 3. Seed Clients
                if (!await context.Clients.AnyAsync())
                {
                    var client1 = new Client { CompanyName = "Innovatech Solutions", ContactPerson = "Jane Doe", Email = "jane.doe@innovatech.com", DateCreated = DateTime.UtcNow };
                    var client2 = new Client { CompanyName = "Quantum Leap Inc.", ContactPerson = "John Smith", Email = "john.smith@quantum.com", DateCreated = DateTime.UtcNow };

                    await context.Clients.AddRangeAsync(client1, client2);
                    await context.SaveChangesAsync();
                }

                // 4. Seed Projects
                if (!await context.Projects.AnyAsync())
                {
                    var teamLead = await userManager.FindByEmailAsync("teamlead@test.com");
                    var client1 = await context.Clients.FirstAsync(c => c.CompanyName == "Innovatech Solutions");
                    var client2 = await context.Clients.FirstAsync(c => c.CompanyName == "Quantum Leap Inc.");

                    var project1 = new Project
                    {
                        Name = "E-commerce Platform Relaunch",
                        Description = "Complete overhaul of the existing e-commerce website.",
                        StartDate = DateTime.UtcNow.AddDays(10),
                        Deadline = DateTime.UtcNow.AddMonths(6),
                        Status = ProjectStatus.Planning,
                        ClientId = client1.Id,
                        TeamLeadId = teamLead.Id,
                        DateCreated = DateTime.UtcNow
                    };

                    var project2 = new Project
                    {
                        Name = "Internal CRM Tool",
                        Description = "Develop a new Customer Relationship Management tool for the sales team.",
                        StartDate = DateTime.UtcNow.AddDays(-30),
                        Deadline = DateTime.UtcNow.AddMonths(3),
                        Status = ProjectStatus.InProgress,
                        ClientId = client2.Id,
                        TeamLeadId = teamLead.Id,
                        DateCreated = DateTime.UtcNow
                    };

                    var project3 = new Project
                    {
                        Name = "Mobile App for Quantum",
                        Description = "Companion mobile app for the main Quantum platform.",
                        StartDate = DateTime.UtcNow.AddDays(-90),
                        Deadline = DateTime.UtcNow.AddDays(-10),
                        Status = ProjectStatus.Completed,
                        ClientId = client2.Id,
                        TeamLeadId = teamLead.Id,
                        DateCreated = DateTime.UtcNow
                    };

                    await context.Projects.AddRangeAsync(project1, project2, project3);
                    await context.SaveChangesAsync();
                }

                // 5. Seed Project Team Members (Many-to-Many)
                if (!await context.Set<ProjectTeamMember>().AnyAsync())
                {
                    var teamLead = await userManager.FindByEmailAsync("teamlead@test.com");
                    var developer = await userManager.FindByEmailAsync("developer@test.com");
                    var crmProject = await context.Projects.FirstAsync(p => p.Name == "Internal CRM Tool");
                    var eCommerceProject = await context.Projects.FirstAsync(p => p.Name == "E-commerce Platform Relaunch");

                    var assignments = new[]
                    {
                    new ProjectTeamMember { ProjectId = crmProject.Id, UserId = developer.Id },
                    new ProjectTeamMember { ProjectId = crmProject.Id, UserId = teamLead.Id },
                    new ProjectTeamMember { ProjectId = eCommerceProject.Id, UserId = developer.Id },
                    };

                    await context.Set<ProjectTeamMember>().AddRangeAsync(assignments);
                    await context.SaveChangesAsync();
                }

                // 6. Seed Tasks
                if (!await context.Tasks.AnyAsync())
                {
                    var developer = await userManager.FindByEmailAsync("developer@test.com");
                    var crmProject = await context.Projects.FirstAsync(p => p.Name == "Internal CRM Tool");

                    var tasks = new[]
                    {
                    new TaskItem { Title = "Design Database Schema", Description = "Plan and create the initial DB schema.", Status = Models.Shared.Enums.TaskStatus.Done, Priority = TaskPriority.High, DueDate = DateTime.UtcNow.AddDays(-10), ProjectId = crmProject.Id, AssignedToId = developer.Id, DateCreated = DateTime.UtcNow },
                    new TaskItem { Title = "Develop Login Page", Description = "Create the user authentication endpoint and UI.", Status = Models.Shared.Enums.TaskStatus.InProgress, Priority = TaskPriority.High, DueDate = DateTime.UtcNow.AddDays(5), ProjectId = crmProject.Id, AssignedToId = developer.Id, DateCreated = DateTime.UtcNow },
                    new TaskItem { Title = "Set Up CI/CD Pipeline", Description = "Configure the continuous integration and deployment workflow.", Status = Models.Shared.Enums.TaskStatus.ToDo, Priority = TaskPriority.Medium, DueDate = DateTime.UtcNow.AddDays(15), ProjectId = crmProject.Id, AssignedToId = null, DateCreated = DateTime.UtcNow },
                    new TaskItem { Title = "Write API Documentation", Description = "Document all public API endpoints.", Status = Models.Shared.Enums.TaskStatus.ToDo, Priority = TaskPriority.Low, DueDate = DateTime.UtcNow.AddDays(20), ProjectId = crmProject.Id, AssignedToId = null, DateCreated = DateTime.UtcNow }
                    };

                    await context.Tasks.AddRangeAsync(tasks);
                    await context.SaveChangesAsync();
                }

                // 7. Seed Time Logs
                if (!await context.TimeLogs.AnyAsync())
                {
                    var developer = await userManager.FindByEmailAsync("developer@test.com");
                    var dbTask = await context.Tasks.FirstAsync(t => t.Title == "Design Database Schema");
                    var loginTask = await context.Tasks.FirstAsync(t => t.Title == "Develop Login Page");

                    var timeLogs = new[]
                    {
                    new TimeLog { Hours = 8, Date = DateTime.UtcNow.AddDays(-11), Description = "Completed initial schema design.", UserId = developer.Id, TaskItemId = dbTask.Id, DateCreated = DateTime.UtcNow },
                    new TimeLog { Hours = 4.5m, Date = DateTime.UtcNow.AddDays(-2), Description = "Backend logic for login endpoint.", UserId = developer.Id, TaskItemId = loginTask.Id, DateCreated = DateTime.UtcNow },
                    new TimeLog { Hours = 3.5m, Date = DateTime.UtcNow.AddDays(-1), Description = "Frontend UI for login form.", UserId = developer.Id, TaskItemId = loginTask.Id, DateCreated = DateTime.UtcNow }
                    };

                    await context.TimeLogs.AddRangeAsync(timeLogs);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
