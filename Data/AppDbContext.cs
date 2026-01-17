using metrica_back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Data
{
    [Controller]
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            // Создание пользователей
            var users = new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "john_doe",
                    Email = "john.doe@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("SecurePass123!"),
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "jane_smith",
                    Email = "jane.smith@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Anna@2024"),
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    UserName = "alex_wong",
                    Email = "alex.wong@example.com",
                    CreatedAt = DateTime.UtcNow.AddDays(-7),
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Qwerty!123"),
                },
            };

            modelBuilder.Entity<User>().HasData(users);
        }
    }
}
