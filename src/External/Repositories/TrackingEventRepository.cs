using ClickHouse.Driver.ADO.Parameters;
using ClickHouse.Driver.Utility;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Enums;
using metrica_back.src.Domain.Models;
using metrica_back.src.External.Databases.ClickHouse;
using Microsoft.Extensions.Options;

namespace metrica_back.src.External.Repositories;

public class TrackingEventRepository(
    ClickHouseContext context,
    IOptions<ClickHouseOptions> options,
    IConfiguration config
) : ITrackingEventRepository
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

        var parameters = new ClickHouseParameterCollection();

        parameters.AddParameter("id", trackingEvent.Id);
        parameters.AddParameter("clientId", trackingEvent.ClientId);
        parameters.AddParameter("sessionId", trackingEvent.SessionId);
        parameters.AddParameter("trackingCode", trackingEvent.TrackingCode);
        parameters.AddParameter("createdAt", trackingEvent.CreatedAt);
        parameters.AddParameter("pageUrl", trackingEvent.PageUrl);
        parameters.AddParameter("pageTitle", trackingEvent.PageTitle);
        parameters.AddParameter("referrer", trackingEvent.Referrer);
        parameters.AddParameter("userAgent", trackingEvent.UserAgent);
        parameters.AddParameter("screenWidth", trackingEvent.ScreenWidth);
        parameters.AddParameter("screenHeight", trackingEvent.ScreenHeight);
        parameters.AddParameter("browserLanguage", trackingEvent.BrowserLanguage);

        await client.ExecuteNonQueryAsync(query, parameters);
    }

    public Task<int> GetTotalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    )
    {
        throw new NotImplementedException();
    }

    public Task<List<IntervalPageViewsDto>> GetIntervalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    )
    {
        throw new NotImplementedException();
    }
}
