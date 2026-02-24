using System.Data;
using AutoMapper;
using ClickHouse.Driver.ADO.Parameters;
using ClickHouse.Driver.Utility;
using metrica_back.Data;
using metrica_back.Models;

// TODO: Конкретизировать типы параметров в query (для производительности)
// TODO: Добавить сохранение пакетов данных (bulk insert)

namespace metrica_back.Repositories
{
    public interface ITrackingEventRepository
    {
        Task CreateTrackingEventAsync(TrackingEvent trackingEvent);
        Task<IEnumerable<TrackingEvent>> GetTrackingEventsByWebsiteIdAsync(
            Guid websiteId,
            DateTime? from = null,
            DateTime? to = null
        );
        Task<IEnumerable<TrackingEvent>> GetTrackingEventsBySessionIdAsync(Guid sessionId);
    }

    public class TrackingEventRepository(ClickHouseContext context, IMapper mapper)
        : ITrackingEventRepository
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

        public async Task<IEnumerable<TrackingEvent>> GetTrackingEventsByWebsiteIdAsync(
            Guid websiteId,
            DateTime? from = null,
            DateTime? to = null
        )
        {
            using var client = context.GetClient();

            var query =
                @"
                SELECT * FROM metrics.tracking_events 
                WHERE WebsiteId = toString(@websiteId)
                AND (@from IS NULL OR Timestamp >= @from)
                AND (@to IS NULL OR Timestamp <= @to)
                ORDER BY Timestamp DESC";

            var parameters = new ClickHouseParameterCollection();

            parameters.AddParameter("websiteId", websiteId);
            parameters.AddParameter("from", from);
            parameters.AddParameter("to", to);

            var reader = await client.ExecuteReaderAsync(query, parameters);

            var results = new List<TrackingEvent>();
            while (await reader.ReadAsync())
            {
                results.Add(mapper.Map<IDataReader, TrackingEvent>(reader));
            }

            return results;
        }

        public async Task<IEnumerable<TrackingEvent>> GetTrackingEventsBySessionIdAsync(
            Guid sessionId
        )
        {
            using var client = context.GetClient();

            var query =
                @"
                SELECT * FROM metrics.tracking_events 
                WHERE SessionId = toString(@sessionId) 
                ORDER BY Timestamp";

            var parameters = new ClickHouseParameterCollection();
            parameters.AddParameter("sessionId", sessionId);

            var reader = await client.ExecuteReaderAsync(query, parameters);

            var results = new List<TrackingEvent>();
            while (await reader.ReadAsync())
            {
                results.Add(mapper.Map<IDataReader, TrackingEvent>(reader));
            }

            return results;
        }
    }
}
