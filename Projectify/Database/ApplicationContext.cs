

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projectify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projectify.Database
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder Builder)
        {
            base.OnModelCreating(Builder);

            Builder.Entity<RoleProjectUser>()
            .HasKey(o => new { o.UserID, o.ProjectID,o.Role });

            Builder.Entity<Project>().HasMany(p => p.Sprints).WithOne(s => s.Project).HasForeignKey(p => p.ProjectID);

        


        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<Models.Task> Tasks { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Sprint> Sprints { get; set; }
        public virtual DbSet<RoleProjectUser> RoleProjectUsers { get; set; }
    }
}
