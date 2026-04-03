using metrica_back.src.Dto;
using metrica_back.src.Models;
using metrica_back.src.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Добавить кеширование результатов аналитических запросов в Redis
// TODO: Добавить авторизацию для владельца сайта

namespace metrica_back.src.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class TrackingEventController(ITrackingEventRepository trackingEventRepository)
{
    [HttpPost]
    public async Task<IResult> CreateTrackingEvent([FromBody] TrackingEventDto trackingEventDto)
    {
        var trackingEvent = new TrackingEvent()
        {
            Id = Guid.NewGuid(),
            ClientId = trackingEventDto.ClientId,
            SessionId = trackingEventDto.SessionId,
            TrackingCode = trackingEventDto.TrackingCode,
            EventType = trackingEventDto.EventType,
            Timestamp = trackingEventDto.Timestamp,
            PageUrl = trackingEventDto.PageUrl,
            PageTitle = trackingEventDto.PageTitle,
            Referrer = trackingEventDto.Referrer,
            UserAgent = trackingEventDto.UserAgent,
            ScreenWidth = trackingEventDto.ScreenWidth,
            ScreenHeight = trackingEventDto.ScreenHeight,
            BrowserLanguage = trackingEventDto.BrowserLanguage,
        };

        await trackingEventRepository.CreateTrackingEventAsync(trackingEvent);

        return Results.Ok(trackingEvent);
    }

    [HttpGet("website/{trackingCode}/page-views")]
    public async Task<IResult> GetPageViews(
        int trackingCode,
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to = null,
        [FromQuery] IntervalType? interval = null
    )
    {
        var totalPageViews = await trackingEventRepository.GetTotalPageViewsAsync(
            trackingCode,
            from,
            to
        );
        var intervalPageViews = await trackingEventRepository.GetIntervalPageViewsAsync(
            trackingCode,
            from,
            to,
            interval
        );
        return Results.Ok(
            new PageViewsResponseDto()
            {
                TotalPageViews = totalPageViews,
                IntervalPageViews = intervalPageViews,
            }
        );
    }
}
