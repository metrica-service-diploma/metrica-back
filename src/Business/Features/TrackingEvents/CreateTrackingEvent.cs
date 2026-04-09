using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Features.TrackingEvents;

public class CreateTrackingEventCommand
    : CreateTrackingEventRequestDto,
        IRequest<Result<TrackingEvent>>
{
    public static CreateTrackingEventCommand FromDto(
        CreateTrackingEventRequestDto trackingEventDto
    ) =>
        new()
        {
            ClientId = trackingEventDto.ClientId,
            SessionId = trackingEventDto.SessionId,
            TrackingCode = trackingEventDto.TrackingCode,
            CreatedAt = trackingEventDto.CreatedAt,
            PageUrl = trackingEventDto.PageUrl,
            PageTitle = trackingEventDto.PageTitle,
            Referrer = trackingEventDto.Referrer,
            UserAgent = trackingEventDto.UserAgent,
            ScreenWidth = trackingEventDto.ScreenWidth,
            ScreenHeight = trackingEventDto.ScreenHeight,
            BrowserLanguage = trackingEventDto.BrowserLanguage,
        };
}

// TODO: Добавить авторизацию для владельца сайта

public class CreateTrackingEventCommandHandler(
    ITrackingEventRepository trackingEventRepository,
    ILogger<CreateTrackingEventCommandHandler> logger
) : IRequestHandler<CreateTrackingEventCommand, Result<TrackingEvent>>
{
    public async Task<Result<TrackingEvent>> Handle(
        CreateTrackingEventCommand request,
        CancellationToken cancellationToken
    )
    {
        var trackingEvent = new TrackingEvent()
        {
            Id = Guid.NewGuid(),
            ClientId = request.ClientId,
            SessionId = request.SessionId,
            TrackingCode = request.TrackingCode,
            CreatedAt = request.CreatedAt,
            PageUrl = request.PageUrl,
            PageTitle = request.PageTitle,
            Referrer = request.Referrer,
            UserAgent = request.UserAgent,
            ScreenWidth = request.ScreenWidth,
            ScreenHeight = request.ScreenHeight,
            BrowserLanguage = request.BrowserLanguage,
        };

        await trackingEventRepository.CreateAsync(trackingEvent);

        logger.LogInformation(
            "Created tracking event {EventId} for tracking code {TrackingCode}",
            trackingEvent.Id,
            trackingEvent.TrackingCode
        );

        return Result<TrackingEvent>.Success(trackingEvent);
    }
}
