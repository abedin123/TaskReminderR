using GenerateSuccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskDB = GenerateSuccess.Models.TaskDB;

namespace GenerateSuccess.AppDBContext
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<TaskDB> Tasks { get; set; }
        public DbSet<Alarms> Alarms { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserAlarm> UserAlarm { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserTask>().HasIndex(b => new { b.TaskId, b.UserId }).IsUnique();
        }
    }
}
