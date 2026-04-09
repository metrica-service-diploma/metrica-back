using MediatR;
using metrica_back.src.Business.Common;
using metrica_back.src.Business.DTOs;
using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Enums;

namespace metrica_back.src.Business.Features.TrackingEvents;

// TODO: Добавить кеширование результатов аналитических запросов в Redis
// TODO: Добавить авторизацию для владельца сайта

public class GetPageViewsQuery(
    int trackingCode,
    DateTime? fromDate,
    DateTime? toDate,
    IntervalType? intervalType
) : IRequest<Result<PageViewsResponseDto>>
{
    public int TrackingCode { get; set; } = trackingCode;
    public DateTime? FromDate { get; set; } = fromDate;
    public DateTime? ToDate { get; set; } = toDate;
    public IntervalType? IntervalType { get; set; } = intervalType;
}

public class GetPageViewsQueryHandler(
    ITrackingEventRepository trackingEventRepository,
    ILogger<GetPageViewsQueryHandler> logger
) : IRequestHandler<GetPageViewsQuery, Result<PageViewsResponseDto>>
{
    public async Task<Result<PageViewsResponseDto>> Handle(
        GetPageViewsQuery request,
        CancellationToken cancellationToken
    )
    {
        var totalPageViews = await trackingEventRepository.GetTotalPageViewsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );
        var intervalPageViews = await trackingEventRepository.GetIntervalPageViewsAsync(
            request.TrackingCode,
            request.FromDate,
            request.ToDate,
            request.IntervalType
        );

        logger.LogInformation(
            "Retrieved page views for tracking code {TrackingCode} from {From} to {To}",
            request.TrackingCode,
            request.FromDate,
            request.ToDate
        );

        return Result<PageViewsResponseDto>.Success(
            new PageViewsResponseDto()
            {
                TotalPageViews = totalPageViews,
                IntervalPageViews = intervalPageViews,
            }
        );
    }
}
