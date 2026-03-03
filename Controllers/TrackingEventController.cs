using metrica_back.Dto;
using metrica_back.Models;
using metrica_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// TODO: Добавить кеширование результатов аналитических запросов в Redis

namespace metrica_back.Controllers
{
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
                SessionId = trackingEventDto.SessionId,
                WebsiteId = trackingEventDto.WebsiteId,
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

        [HttpGet("website/{websiteId}/page-views")]
        public async Task<IResult> GetPageViews(
            Guid websiteId,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null,
            [FromQuery] IntervalType? interval = null
        )
        {
            var totalPageViews = await trackingEventRepository.GetTotalPageViewsAsync(
                websiteId,
                from,
                to
            );
            var intervalPageViews = await trackingEventRepository.GetIntervalPageViewsAsync(
                websiteId,
                from,
                to,
                interval
            );
            return Results.Ok(
                new PageViewsResponseDto()
                {
                    WebsiteId = websiteId,
                    TotalPageViews = totalPageViews,
                    IntervalPageViews = intervalPageViews,
                }
            );
        }
    }
}
