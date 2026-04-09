using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Helpers;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Enums;
using metrica_back.src.Domain.Models;
using metrica_back.src.External.Databases.ClickHouse;
using metrica_back.src.External.Helpers;
using Microsoft.Extensions.Options;

namespace metrica_back.src.External.Repositories;

public class TrackingEventRepository(ClickHouseContext context, IOptions<ClickHouseOptions> options)
    : ITrackingEventRepository
{
    public async Task CreateAsync(TrackingEvent trackingEvent)
    {
        using var client = context.GetClient();

        var query =
            $@"
            INSERT INTO {options.Value.GetFullTableName()} (
                Id, ClientId, SessionId, TrackingCode, 
                CreatedAt, PageUrl, PageTitle, Referrer, UserAgent, 
                ScreenWidth, ScreenHeight, BrowserLanguage
            ) VALUES (
                @id, @clientId, @sessionId, @trackingCode, 
                @createdAt, @pageUrl, @pageTitle, @referrer, @userAgent, 
                @screenWidth, @screenHeight, @browserLanguage
            )";

        await client.ExecuteNonQueryAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("id", trackingEvent.Id),
                ("clientId", trackingEvent.ClientId),
                ("sessionId", trackingEvent.SessionId),
                ("trackingCode", trackingEvent.TrackingCode),
                ("createdAt", trackingEvent.CreatedAt),
                ("pageUrl", trackingEvent.PageUrl),
                ("pageTitle", trackingEvent.PageTitle),
                ("referrer", trackingEvent.Referrer),
                ("userAgent", trackingEvent.UserAgent),
                ("screenWidth", trackingEvent.ScreenWidth),
                ("screenHeight", trackingEvent.ScreenHeight),
                ("browserLanguage", trackingEvent.BrowserLanguage)
            )
        );
    }

    public async Task<int> GetTotalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    )
    {
        using var client = context.GetClient();

        var query =
            $@"
            SELECT COUNT(*) as TotalPageViews
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
                AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
                AND (@toDate IS NULL OR CreatedAt <= @toDate)";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        return await reader.ReadAsync()
            ? (int)reader.GetUInt64(reader.GetOrdinal("TotalPageViews"))
            : 0;
    }

    public async Task<List<IntervalPageViewsDto>> GetIntervalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    )
    {
        using var client = context.GetClient();

        var (intervalValue, intervalType) = TrackingEventsHelper.GetIntervalParameters(
            interval ?? IntervalType.Weeks
        );

        var query =
            $@"
            SELECT COUNT(*) as IntervalPageViews,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) as IntervalStart,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) 
                    + INTERVAL {intervalValue} {intervalType} as IntervalEnd
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
                AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
                AND (@toDate IS NULL OR CreatedAt <= @toDate)
            GROUP BY IntervalStart, IntervalEnd
            ORDER BY IntervalStart";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        var result = new List<IntervalPageViewsDto>();

        while (await reader.ReadAsync())
        {
            result.Add(
                new IntervalPageViewsDto
                {
                    PageViews = (int)reader.GetUInt64(reader.GetOrdinal("IntervalPageViews")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("IntervalStart")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("IntervalEnd")),
                }
            );
        }

        return result;
    }

    public async Task<int> GetTotalVisitsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    )
    {
        using var client = context.GetClient();

        var query =
            $@"
            SELECT COUNT(DISTINCT SessionId) as TotalVisits
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
            AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
            AND (@toDate IS NULL OR CreatedAt <= @toDate)";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        return await reader.ReadAsync()
            ? (int)reader.GetUInt64(reader.GetOrdinal("TotalVisits"))
            : 0;
    }

    public async Task<List<IntervalVisitsDto>> GetIntervalVisitsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    )
    {
        using var client = context.GetClient();

        var (intervalValue, intervalType) = TrackingEventsHelper.GetIntervalParameters(
            interval ?? IntervalType.Weeks
        );

        var query =
            $@"
            SELECT COUNT(DISTINCT SessionId) as IntervalVisits,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) as IntervalStart,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) 
                    + INTERVAL {intervalValue} {intervalType} as IntervalEnd
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
                AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
                AND (@toDate IS NULL OR CreatedAt <= @toDate)
            GROUP BY IntervalStart, IntervalEnd
            ORDER BY IntervalStart";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        var result = new List<IntervalVisitsDto>();

        while (await reader.ReadAsync())
        {
            result.Add(
                new IntervalVisitsDto
                {
                    Visits = (int)reader.GetUInt64(reader.GetOrdinal("IntervalVisits")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("IntervalStart")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("IntervalEnd")),
                }
            );
        }

        return result;
    }

    public async Task<int> GetTotalVisitorsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    )
    {
        using var client = context.GetClient();

        var query =
            $@"
            SELECT COUNT(DISTINCT ClientId) as TotalVisitors
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
            AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
            AND (@toDate IS NULL OR CreatedAt <= @toDate)";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        return await reader.ReadAsync()
            ? (int)reader.GetUInt64(reader.GetOrdinal("TotalVisitors"))
            : 0;
    }

    public async Task<List<IntervalVisitorsDto>> GetIntervalVisitorsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    )
    {
        using var client = context.GetClient();

        var (intervalValue, intervalType) = TrackingEventsHelper.GetIntervalParameters(
            interval ?? IntervalType.Weeks
        );

        var query =
            $@"
            SELECT COUNT(DISTINCT ClientId) as IntervalVisitors,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) as IntervalStart,
                toStartOfInterval(CreatedAt, INTERVAL {intervalValue} {intervalType}) 
                    + INTERVAL {intervalValue} {intervalType} as IntervalEnd
            FROM {options.Value.GetFullTableName()}
            WHERE TrackingCode = @trackingCode
                AND (@fromDate IS NULL OR CreatedAt >= @fromDate)
                AND (@toDate IS NULL OR CreatedAt <= @toDate)
            GROUP BY IntervalStart, IntervalEnd
            ORDER BY IntervalStart";

        var reader = await client.ExecuteReaderAsync(
            query,
            ClickHouseHelper.CreateParameters(
                ("trackingCode", trackingCode),
                ("fromDate", fromDate),
                ("toDate", toDate)
            )
        );

        var result = new List<IntervalVisitorsDto>();

        while (await reader.ReadAsync())
        {
            result.Add(
                new IntervalVisitorsDto
                {
                    Visitors = (int)reader.GetUInt64(reader.GetOrdinal("IntervalVisitors")),
                    StartDate = reader.GetDateTime(reader.GetOrdinal("IntervalStart")),
                    EndDate = reader.GetDateTime(reader.GetOrdinal("IntervalEnd")),
                }
            );
        }

        return result;
    }
}
