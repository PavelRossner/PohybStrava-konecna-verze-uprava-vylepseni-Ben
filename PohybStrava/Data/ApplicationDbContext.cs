using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using PohybStrava.Models;
using PohybStrava.Models.Response;

namespace PohybStrava.Data
{

    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Diet> Diet { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Stats> Stats { get; set; }
        public DbSet<FoodDatabase> FoodDatabase { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Activity>()
            .HasOne(c => c.User)
            .WithMany(t => t.Activities)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Diet>()
            .HasOne(c => c.User)
            .WithMany(t => t.Diet)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Stats>()
            .HasOne(c => c.User)
            .WithMany(t => t.Stats)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FoodDatabase>()
            .HasOne(c => c.User)
            .WithMany(t => t.FoodDatabase)
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        }

    }
}