using metrica_back.src.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.Data;

public class PostgreSQLContext(DbContextOptions<PostgreSQLContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Website> Websites { get; set; }

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
