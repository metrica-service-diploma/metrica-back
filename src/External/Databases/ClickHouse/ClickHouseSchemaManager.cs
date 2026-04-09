using ClickHouse.Driver;
using metrica_back.src.External.Interfaces;
using Microsoft.Extensions.Options;

namespace metrica_back.src.External.Databases.ClickHouse;

public class ClickHouseSchemaManager(IOptions<ClickHouseOptions> options) : IClickHouseSchemaManager
{
    public async Task<bool> DatabaseExistsAsync(ClickHouseClient client)
    {
        var result = await client.ExecuteScalarAsync(
            $"EXISTS DATABASE {options.Value.DatabaseName}"
        );
        return Convert.ToBoolean(result);
    }

    public async Task<bool> TableExistsAsync(ClickHouseClient client)
    {
        var result = await client.ExecuteScalarAsync(
            $"EXISTS TABLE {options.Value.GetFullTableName()}"
        );
        return Convert.ToBoolean(result);
    }

    public async Task CreateDatabaseIfNotExistsAsync(ClickHouseClient client)
    {
        await client.ExecuteNonQueryAsync(
            $"CREATE DATABASE IF NOT EXISTS {options.Value.DatabaseName}"
        );
    }

    public async Task CreateTableIfNotExistsAsync(ClickHouseClient client)
    {
        await client.ExecuteNonQueryAsync(
            $@"
            CREATE TABLE IF NOT EXISTS {options.Value.GetFullTableName()} (
                Id String,
                ClientId String,
                SessionId String,
                TrackingCode Int32,
                CreatedAt DateTime,
                PageUrl Nullable(String),
                PageTitle Nullable(String),
                Referrer Nullable(String),
                UserAgent Nullable(String),
                ScreenWidth Int32,
                ScreenHeight Int32,
                BrowserLanguage Nullable(String)
            ) ENGINE = MergeTree()
            ORDER BY CreatedAt"
        );
    }
}
