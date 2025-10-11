using DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            // --- Configure Many-to-Many for Project and User (Team Members) ---
            modelBuilder.Entity<ProjectTeamMember>()
                .HasKey(pt => new { pt.ProjectId, pt.UserId }); // Composite primary key

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(pt => pt.Project)
                .WithMany(p => p.TeamMembers)
                .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectTeamMember>()
                .HasOne(pt => pt.User)
                .WithMany(u => u.ProjectAssignments)
                .HasForeignKey(pt => pt.UserId);


            // --- Configure One-to-Many for Project and User (Team Lead) ---
            modelBuilder.Entity<Project>()
                .HasOne(p => p.TeamLead)
                .WithMany(u => u.LedProjects)
                .HasForeignKey(p => p.TeamLeadId)
                .OnDelete(DeleteBehavior.Restrict); // Prevents deleting a user if they are a team lead


            // --- Configure One-to-Many for Task and User (Assigned To) ---
            modelBuilder.Entity<TaskItem>()
                .HasOne(t => t.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull); // If a user is deleted, set AssignedToId to null

            // Configure the decimal precision for the TimeLog.Hours property
            modelBuilder.Entity<TimeLog>(entity =>
            {
                entity.Property(e => e.Hours).HasColumnType("decimal(10,2)");
            });

        }
    }
}
