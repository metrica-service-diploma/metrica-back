using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Enums;

namespace metrica_back.src.Business.Features.TrackingEvents;

// TODO: Добавить кеширование результатов аналитических запросов в Redis
// TODO: Добавить авторизацию для владельца сайта

public class GetVisitorsQuery(
    int trackingCode,
    DateTime? fromDate,
    DateTime? toDate,
    IntervalType? intervalType
) : IRequest<Result<VisitorsResponseDto>>
{
    public int TrackingCode { get; set; } = trackingCode;
    public DateTime? FromDate { get; set; } = fromDate;
    public DateTime? ToDate { get; set; } = toDate;
    public IntervalType? IntervalType { get; set; } = intervalType;
}

public class GetVisitorsQueryHandler(
    ITrackingEventRepository trackingEventRepository,
    ILogger<GetVisitorsQueryHandler> logger
) : IRequestHandler<GetVisitorsQuery, Result<VisitorsResponseDto>>
{
    public async Task<Result<VisitorsResponseDto>> Handle(
        GetVisitorsQuery request,
        CancellationToken cancellationToken
    )
    {
        var totalVisitors = await trackingEventRepository.GetTotalVisitorsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );
        var intervalVisitors = await trackingEventRepository.GetIntervalVisitorsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate,
            request.IntervalType
        );

        logger.LogInformation(
            "Retrieved visitors for tracking code {TrackingCode} from {From} to {To}",
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );

        return Result<VisitorsResponseDto>.Success(
            new VisitorsResponseDto()
            {
                TotalVisitors = totalVisitors,
                IntervalVisitors = intervalVisitors,
            }
        );
    }
}
