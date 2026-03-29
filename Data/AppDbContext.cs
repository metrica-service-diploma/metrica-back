using metrica_back.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Data
{
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
            modelBuilder.Entity<User>().HasData(TestData.Users);
            modelBuilder.Entity<Website>().HasData(TestData.Websites);
        }
    }
}
