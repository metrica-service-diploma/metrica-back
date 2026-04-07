using metrica_back.src.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External.Databases.PostgreSql;

public class PostgreSqlContext(DbContextOptions<PostgreSqlContext> options) : DbContext(options)
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
    }
}
