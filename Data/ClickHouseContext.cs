using ClickHouse.Driver;

namespace metrica_back.Data
{
    public class ClickHouseContext
    {
        private readonly string _connectionString;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ClickHouseContext(IConfiguration configuration, ILogger<ClickHouseContext> logger)
        {
            _connectionString = configuration.GetConnectionString("ClickHouseConnection");
            _configuration = configuration;
            _logger = logger;
        }

        public async Task InitializeAsync()
        {
            var databaseName = _configuration["ClickHouse:DatabaseName"].ToString();

            try
            {
                await CreateDatabaseIfNotExistsAsync(databaseName);
                await CreateTablesIfNotExistsAsync(databaseName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to initialize database '{databaseName}'");
                throw;
            }
        }

        public ClickHouseClient GetClient() => new(_connectionString);

        private async Task CreateDatabaseIfNotExistsAsync(string databaseName)
        {
            using var client = GetClient();

            var createDatabaseQuery = $@"CREATE DATABASE IF NOT EXISTS {databaseName}";

            await client.ExecuteNonQueryAsync(createDatabaseQuery);
        }

        private async Task CreateTablesIfNotExistsAsync(string databaseName)
        {
            using var client = GetClient();

            var createTableQuery =
                $@"
                CREATE TABLE IF NOT EXISTS {databaseName}.tracking_events (
                    Id String,
                    SessionId String,
                    WebsiteId String,
                    EventType String,
                    Timestamp DateTime,
                    PageUrl String,
                    PageTitle String,
                    Referrer String,
                    UserAgent String,
                    ScreenWidth Int32,
                    ScreenHeight Int32,
                    BrowserLanguage String
                ) ENGINE = MergeTree()
                ORDER BY Timestamp";

            await client.ExecuteNonQueryAsync(createTableQuery);
        }
    }
}
