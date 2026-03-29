using AutoMapper;
using ClickHouse.Driver;
using ClickHouse.Driver.Copy;

namespace metrica_back.Data
{
    public class ClickHouseContext(
        IConfiguration config,
        ILogger<ClickHouseContext> logger,
        IMapper mapper
    )
    {
        private readonly string databaseName = config["ClickHouse:DatabaseName"];
        private readonly string tableName = config["ClickHouse:TableName"];

        public async Task InitializeAsync()
        {
            try
            {
                using var client = GetClient();

                if (!await IsDatabaseExists(client))
                {
                    await CreateDatabase(client);
                    logger.LogInformation("Created ClickHouse database '{Database}'", databaseName);
                }

                if (!await IsTableExists(client))
                {
                    await CreateTable(client);
                    await SeedData(client);

                    logger.LogInformation(
                        "Created ClickHouse table '{Table}' with seed data",
                        tableName
                    );
                }
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Failed to initialize ClickHouse database '{Database}'",
                    databaseName
                );
                throw;
            }
        }

        public ClickHouseClient GetClient() =>
            new(config.GetConnectionString("ClickHouseConnection"));

        private async Task<bool> IsDatabaseExists(ClickHouseClient client)
        {
            var result = await client.ExecuteScalarAsync($"EXISTS DATABASE {databaseName}");
            return Convert.ToBoolean(result);
        }

        private async Task<bool> IsTableExists(ClickHouseClient client)
        {
            var result = await client.ExecuteScalarAsync(
                $"EXISTS TABLE {databaseName}.{tableName}"
            );
            return Convert.ToBoolean(result);
        }

        private async Task CreateDatabase(ClickHouseClient client)
        {
            await client.ExecuteNonQueryAsync($"CREATE DATABASE IF NOT EXISTS {databaseName}");
        }

        private async Task CreateTable(ClickHouseClient client)
        {
            await client.ExecuteNonQueryAsync(
                $@"
                CREATE TABLE IF NOT EXISTS {databaseName}.{tableName} (
                    Id String,
                    SessionId String,
                    WebsiteId String,
                    EventType Nullable(String),
                    Timestamp DateTime,
                    PageUrl Nullable(String),
                    PageTitle Nullable(String),
                    Referrer Nullable(String),
                    UserAgent Nullable(String),
                    ScreenWidth Int32,
                    ScreenHeight Int32,
                    BrowserLanguage Nullable(String)
                ) ENGINE = MergeTree()
                ORDER BY Timestamp"
            );
        }

        private async Task SeedData(ClickHouseClient client)
        {
            var rows = TestData
                .TrackingEvents.Select(e =>
                    new object[]
                    {
                        e.Id.ToString(),
                        e.SessionId.ToString(),
                        e.WebsiteId.ToString(),
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
                    "SessionId",
                    "WebsiteId",
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
}
