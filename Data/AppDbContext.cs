using metrica_back.Helpers;
using metrica_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Data
{
    [Controller]
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Website> Websites { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            // Database.EnsureDeleted();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Websites)
                .WithOne(w => w.User)
                .HasForeignKey(w => w.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            var users = new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "john_doe",
                    Email = "john.doe@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    PasswordHash = PasswordHasher.HashPassword("SecurePass123!"),
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "jane_smith",
                    Email = "jane.smith@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    PasswordHash = PasswordHasher.HashPassword("Anna@2024"),
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "alex_wong",
                    Email = "alex.wong@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    PasswordHash = PasswordHasher.HashPassword("Qwerty!123"),
                },
            };

            modelBuilder.Entity<User>().HasData(users);

            var websites = new[]
            {
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 1",
                    Domain = "my.website1.com",
                    TrackingCode = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UserId = users[0].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 2",
                    Domain = "my.website2.com",
                    TrackingCode = 2,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UserId = users[0].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 3",
                    Domain = "my.website3.com",
                    TrackingCode = 3,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UserId = users[0].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 4",
                    Domain = "my.website4.com",
                    TrackingCode = 4,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UserId = users[1].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 5",
                    Domain = "my.website5.com",
                    TrackingCode = 5,
                    CreatedAt = DateTime.UtcNow.AddDays(-1),
                    UserId = users[1].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 6",
                    Domain = "my.website6.com",
                    TrackingCode = 6,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    UserId = users[1].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 7",
                    Domain = "my.website7.com",
                    TrackingCode = 7,
                    CreatedAt = DateTime.UtcNow.AddDays(-4),
                    UserId = users[2].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 8",
                    Domain = "my.website8.com",
                    TrackingCode = 8,
                    CreatedAt = DateTime.UtcNow.AddDays(-3),
                    UserId = users[2].Id,
                },
                new Website
                {
                    Id = Guid.NewGuid(),
                    Name = "Website 9",
                    Domain = "my.website9.com",
                    TrackingCode = 9,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UserId = users[2].Id,
                },
            };

            modelBuilder.Entity<Website>().HasData(websites);
        }
    }
}
