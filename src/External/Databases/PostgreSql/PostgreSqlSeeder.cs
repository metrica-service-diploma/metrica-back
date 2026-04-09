using metrica_back.src.Domain.Models;
using metrica_back.src.External.Common;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External.Databases.PostgreSql;

public class PostgreSqlSeeder(PostgreSqlContext postgreSqlContext, ILogger<PostgreSqlSeeder> logger)
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting PostgreSql database seeding...");

        if (await postgreSqlContext.Users.AnyAsync())
        {
            logger.LogInformation("PostgreSql database already contains data. Skipping seed.");
            return;
        }

        (User[] users, Website[] websites) = PostgreSqlTestDataFactory.CreateTestData();

        await postgreSqlContext.Users.AddRangeAsync(users);
        await postgreSqlContext.Websites.AddRangeAsync(websites);
        await postgreSqlContext.SaveChangesAsync();

        logger.LogInformation("PostgreSql database seeding completed successfully.");
    }
}
