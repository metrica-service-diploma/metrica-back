using ClickHouse.Driver;
using ClickHouse.Driver.Copy;
using metrica_back.src.Domain.Models;
using metrica_back.src.External.Common;
using Microsoft.Extensions.Options;

namespace metrica_back.src.External.Databases.ClickHouse;

public class ClickHouseSeeder(
    ClickHouseContext clickHouseContext,
    IOptions<ClickHouseOptions> options,
    ILogger<ClickHouseSeeder> logger
)
{
    public async Task SeedAsync()
    {
        logger.LogInformation("Starting ClickHouse database seeding...");

        using var client = clickHouseContext.GetClient();

        if (await HasDataAsync(client))
        {
            logger.LogInformation("ClickHouse database already contains data. Skipping seed.");
            return;
        }

        List<TrackingEvent> trackingEvents = ClickHouseTestDataFactory.CreateTestData();

        await InsertDataAsync(client, trackingEvents);

        logger.LogInformation("ClickHouse database seeding completed successfully.");
    }

    public async Task<bool> HasDataAsync(ClickHouseClient client)
    {
        var result = await client.ExecuteScalarAsync(
            $"SELECT COUNT(*) FROM {options.Value.GetFullTableName()}"
        );
        return Convert.ToInt64(result) > 0;
    }

    public async Task InsertDataAsync(ClickHouseClient client, List<TrackingEvent> trackingEvents)
    {
        var rows = trackingEvents
            .Select(e =>
                new object[]
                {
                    e.Id.ToString(),
                    e.ClientId.ToString(),
                    e.SessionId.ToString(),
                    e.TrackingCode,
                    e.CreatedAt,
                    e.PageUrl,
                    e.PageTitle,
                    e.Referrer,
                    e.UserAgent,
                    e.ScreenWidth,
                    e.ScreenHeight,
                    e.BrowserLanguage,
                }
            )
            .ToArray();

        await client.InsertBinaryAsync(
            $"{options.Value.GetFullTableName()}",
            [
                "Id",
                "ClientId",
                "SessionId",
                "TrackingCode",
                "CreatedAt",
                "PageUrl",
                "PageTitle",
                "Referrer",
                "UserAgent",
                "ScreenWidth",
                "ScreenHeight",
                "BrowserLanguage",
            ],
            rows,
            new() { Format = RowBinaryFormat.RowBinaryWithDefaults }
        );
    }
}
