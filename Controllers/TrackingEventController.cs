using metrica_back.Dto;
using metrica_back.Models;
using metrica_back.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("website/{websiteId}")]
        public async Task<IResult> GetTrackingEventsByWebsiteId(
            Guid websiteId,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null
        )
        {
            var events = await trackingEventRepository.GetTrackingEventsByWebsiteIdAsync(
                websiteId,
                from,
                to
            );
            return Results.Ok(events);
        }

        [HttpGet("session/{sessionId}")]
        public async Task<IResult> GetTrackingEventsBySessionId(Guid sessionId)
        {
            var events = await trackingEventRepository.GetTrackingEventsBySessionIdAsync(sessionId);
            return Results.Ok(events);
        }
    }
}
