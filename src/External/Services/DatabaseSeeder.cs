using metrica_back.src.Business.Helpers;
using metrica_back.src.Core.Interfaces.Services;
using metrica_back.src.Core.Models;
using metrica_back.src.External.Databases.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External.Services;

public class DatabaseSeeder(PostgreSqlContext postgreSqlContext, ILogger<DatabaseSeeder> logger)
    : IDatabaseSeeder
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting database seeding...");

        if (await postgreSqlContext.Users.AnyAsync())
        {
            logger.LogInformation("Database already contains data. Skipping seed.");
            return;
        }

        await SeedDevelopmentDataAsync();

        logger.LogInformation("Database seeding completed successfully.");
    }

    private async Task SeedDevelopmentDataAsync()
    {
        logger.LogInformation("Seeding development data...");

        var userId = Guid.Parse("11111111-1111-1111-1111-111111111111");

        var user = new User
        {
            Id = userId,
            UserName = "john_doe",
            Email = "john.doe@example.com",
            CreatedAt = DateTime.UtcNow.AddDays(-30),
            PasswordHash = PasswordHasher.Hash("SecurePass123!"),
        };

        var websites = new List<Website>
        {
            new()
            {
                Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Интернет-магазин",
                Domain = "shop.example.com",
                TrackingCode = 1,
                CreatedAt = DateTime.UtcNow.AddDays(-5),
                UserId = userId,
            },
            new()
            {
                Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Name = "Блог",
                Domain = "blog.example.com",
                TrackingCode = 2,
                CreatedAt = DateTime.UtcNow.AddDays(-4),
                UserId = userId,
            },
        };

        await postgreSqlContext.Users.AddAsync(user);
        await postgreSqlContext.Websites.AddRangeAsync(websites);
        await postgreSqlContext.SaveChangesAsync();

        // await SeedTrackingEventsAsync();

        logger.LogInformation("Development data seeded successfully.");
    }

    // private async Task SeedTrackingEventsAsync()
    // {
    //     var testData = TestDataFactory.CreateTestData();

    //     // Получаем существующие сайты
    //     var shop = await postgreSqlContext.Websites.FirstAsync(w => w.TrackingCode == 1);
    //     var blog = await postgreSqlContext.Websites.FirstAsync(w => w.TrackingCode == 2);

    //     // Обновляем TrackingCode в событиях
    //     foreach (var evt in testData)
    //     {
    //         evt.TrackingCode = evt.PageUrl.Contains("shop") ? shop.TrackingCode : blog.TrackingCode;
    //     }

    //     // await postgreSqlContext.TrackingEvents.AddRangeAsync(testData);
    //     await postgreSqlContext.SaveChangesAsync();
    // }
}
