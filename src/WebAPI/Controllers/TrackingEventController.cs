using MediatR;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Features.TrackingEvents;
using metrica_back.src.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace metrica_back.src.WebAPI.Controllers;

[ApiController]
[Route("api/events")]
[Authorize]
public class TrackingEventController(IMediator mediator)
{
    [HttpPost]
    public async Task<IResult> CreateTrackingEvent(
        [FromBody] CreateTrackingEventRequestDto trackingEventRequestDto
    )
    {
        var result = await mediator.Send(
            CreateTrackingEventCommand.FromDto(trackingEventRequestDto)
        );

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }

    [HttpGet("website/{trackingCode}/page-views")]
    public async Task<IResult> GetPageViews(
        int trackingCode,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] IntervalType? intervalType = null
    )
    {
        var result = await mediator.Send(
            new GetPageViewsQuery(trackingCode, fromDate, toDate, intervalType)
        );

        return result.IsSuccess
            ? Results.Ok(result.Data)
            : Results.Json(new { message = result.Error }, statusCode: result.StatusCode);
    }
}
