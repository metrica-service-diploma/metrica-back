using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Enums;

namespace metrica_back.src.Business.Features.TrackingEvents;

// TODO: Добавить кеширование результатов аналитических запросов в Redis
// TODO: Добавить авторизацию для владельца сайта

public class GetVisitsQuery(
    int trackingCode,
    DateTime? fromDate,
    DateTime? toDate,
    IntervalType? intervalType
) : IRequest<Result<VisitsResponseDto>>
{
    public int TrackingCode { get; set; } = trackingCode;
    public DateTime? FromDate { get; set; } = fromDate;
    public DateTime? ToDate { get; set; } = toDate;
    public IntervalType? IntervalType { get; set; } = intervalType;
}

public class GetVisitsQueryHandler(
    ITrackingEventRepository trackingEventRepository,
    ILogger<GetVisitsQueryHandler> logger
) : IRequestHandler<GetVisitsQuery, Result<VisitsResponseDto>>
{
    public async Task<Result<VisitsResponseDto>> Handle(
        GetVisitsQuery request,
        CancellationToken cancellationToken
    )
    {
        var totalVisits = await trackingEventRepository.GetTotalVisitsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );
        var intervalVisits = await trackingEventRepository.GetIntervalVisitsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate,
            request.IntervalType
        );

        logger.LogInformation(
            "Retrieved visits for tracking code {TrackingCode} from {From} to {To}",
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );

        return Result<VisitsResponseDto>.Success(
            new VisitsResponseDto() { TotalVisits = totalVisits, IntervalVisits = intervalVisits }
        );
    }
}
