using ClickHouse.Driver;
using ClickHouse.Driver.Copy;

namespace metrica_back.src.Data.ClickHouse;

public interface IClickHouseDataSeeder
{
    Task SeedIfEmptyAsync(ClickHouseClient client, string databaseName, string tableName);
}

public class ClickHouseDataSeeder(ILogger<ClickHouseDataSeeder> logger) : IClickHouseDataSeeder
{
    public async Task SeedIfEmptyAsync(
        ClickHouseClient client,
        string databaseName,
        string tableName
    )
    {
        var count = await GetRowCount(client, databaseName, tableName);
        if (count > 0)
            return;

        await InsertTestData(client, databaseName, tableName);
        logger.LogInformation(
            "Seeded {Count} rows into {Table}",
            TestData.TrackingEvents.Count,
            $"{databaseName}.{tableName}"
        );
    }

    private static async Task<long> GetRowCount(
        ClickHouseClient client,
        string databaseName,
        string tableName
    )
    {
        var result = await client.ExecuteScalarAsync(
            $"SELECT COUNT(*) FROM {databaseName}.{tableName}"
        );
        return Convert.ToInt64(result);
    }

    private static async Task InsertTestData(
        ClickHouseClient client,
        string databaseName,
        string tableName
    )
    {
        var rows = TestData
            .TrackingEvents.Select(e =>
                new object[]
                {
                    e.Id.ToString(),
                    e.ClientId.ToString(),
                    e.SessionId.ToString(),
                    e.TrackingCode,
                    e.EventType,
                    e.Timestamp,
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
            $"{databaseName}.{tableName}",
            [
                "Id",
                "ClientId",
                "SessionId",
                "TrackingCode",
                "EventType",
                "Timestamp",
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
