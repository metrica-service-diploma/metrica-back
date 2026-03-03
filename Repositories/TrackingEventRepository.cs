using ClickHouse.Driver.ADO.Parameters;
using ClickHouse.Driver.Utility;
using metrica_back.Data;
using metrica_back.Dto;
using metrica_back.Helpers;
using metrica_back.Models;

// TODO: Конкретизировать типы параметров в query (для производительности)
// TODO: Добавить сохранение пакетов данных (bulk insert)

namespace metrica_back.Repositories
{
    public interface ITrackingEventRepository
    {
        Task CreateTrackingEventAsync(TrackingEvent trackingEvent);
        Task<int> GetTotalPageViewsAsync(
            Guid websiteId,
            DateTime? from = null,
            DateTime? to = null
        );
        Task<List<IntervalPageViews>> GetIntervalPageViewsAsync(
            Guid websiteId,
            DateTime? from,
            DateTime? to,
            IntervalType? interval
        );
    }

    public class TrackingEventRepository(ClickHouseContext context) : ITrackingEventRepository
    {
        public async Task CreateTrackingEventAsync(TrackingEvent trackingEvent)
        {
            using var client = context.GetClient();

            var query =
                @"
                INSERT INTO metrics.tracking_events (
                    Id, SessionId, WebsiteId, EventType, Timestamp, 
                    PageUrl, PageTitle, Referrer, UserAgent, 
                    ScreenWidth, ScreenHeight, BrowserLanguage
                ) VALUES (
                    {id: String}, 
                    {sessionId: String}, 
                    {websiteId: String}, 
                    {eventType: String}, 
                    {timestamp: DateTime}, 
                    {pageUrl: String}, 
                    {pageTitle: String}, 
                    {referrer: String}, 
                    {userAgent: String}, 
                    {screenWidth: Int32}, 
                    {screenHeight: Int32}, 
                    {browserLanguage: String}
                )";

            var parameters = new ClickHouseParameterCollection();

            parameters.AddParameter("id", trackingEvent.Id);
            parameters.AddParameter("sessionId", trackingEvent.SessionId);
            parameters.AddParameter("websiteId", trackingEvent.WebsiteId);
            parameters.AddParameter("eventType", trackingEvent.EventType);
            parameters.AddParameter("timestamp", trackingEvent.Timestamp);
            parameters.AddParameter("pageUrl", trackingEvent.PageUrl);
            parameters.AddParameter("pageTitle", trackingEvent.PageTitle);
            parameters.AddParameter("referrer", trackingEvent.Referrer);
            parameters.AddParameter("userAgent", trackingEvent.UserAgent);
            parameters.AddParameter("screenWidth", trackingEvent.ScreenWidth);
            parameters.AddParameter("screenHeight", trackingEvent.ScreenHeight);
            parameters.AddParameter("browserLanguage", trackingEvent.BrowserLanguage);

            await client.ExecuteNonQueryAsync(query, parameters);
        }

        public async Task<int> GetTotalPageViewsAsync(
            Guid websiteId,
            DateTime? from = null,
            DateTime? to = null
        )
        {
            using var client = context.GetClient();

            var query =
                @"
                SELECT 
                    WebsiteId,
                    count(*) as TotalPageViews
                FROM metrics.tracking_events
                WHERE 
                    WebsiteId = toString(@websiteId) 
                    AND EventType = 'page_view'
                    AND (@from IS NULL OR Timestamp >= @from)
                    AND (@to IS NULL OR Timestamp <= @to)
                GROUP BY WebsiteId";

            var parameters = new ClickHouseParameterCollection();

            parameters.AddParameter("websiteId", websiteId);
            parameters.AddParameter("from", from);
            parameters.AddParameter("to", to);

            var reader = await client.ExecuteReaderAsync(query, parameters);
            if (!await reader.ReadAsync())
                return 0;

            return Convert.ToInt32(reader["TotalPageViews"]);
        }

        public async Task<List<IntervalPageViews>> GetIntervalPageViewsAsync(
            Guid websiteId,
            DateTime? from = null,
            DateTime? to = null,
            IntervalType? interval = IntervalType.Weeks
        )
        {
            using var client = context.GetClient();

            var (intervalValue, intervalType) = TrackingEventsHelper.GetIntervalParameters(
                interval ?? IntervalType.Weeks
            );

            var query =
                $@"
                SELECT 
                    WebsiteId,
                    toStartOfInterval(Timestamp, INTERVAL {intervalValue} {intervalType}) as IntervalStart,
                    toStartOfInterval(Timestamp, INTERVAL {intervalValue} {intervalType}) + INTERVAL {intervalValue} {intervalType} as IntervalEnd,
                    count(*) as PageViews
                FROM metrics.tracking_events
                WHERE 
                    WebsiteId = toString(@websiteId) 
                    AND EventType = 'page_view'
                    AND (@from IS NULL OR Timestamp >= @from)
                    AND (@to IS NULL OR Timestamp <= @to)
                GROUP BY WebsiteId, IntervalStart, IntervalEnd
                ORDER BY IntervalStart";

            var parameters = new ClickHouseParameterCollection();

            parameters.AddParameter("websiteId", websiteId);
            parameters.AddParameter("from", from);
            parameters.AddParameter("to", to);

            parameters.AddParameter("intervalValue", intervalValue);
            parameters.AddParameter("intervalType", intervalType);

            var reader = await client.ExecuteReaderAsync(query, parameters);
            var result = new List<IntervalPageViews>();

            while (await reader.ReadAsync())
            {
                result.Add(
                    new IntervalPageViews
                    {
                        PageViews = Convert.ToInt32(reader["PageViews"]),
                        StartDate = Convert.ToDateTime(reader["IntervalStart"]),
                        EndDate = Convert.ToDateTime(reader["IntervalEnd"]),
                    }
                );
            }

            return result;
        }
    }
}
