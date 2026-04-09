using metrica_back.src.Business.DTOs;
using metrica_back.src.Domain.Enums;
using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Interfaces.Repositories;

public interface ITrackingEventRepository
{
    Task CreateAsync(TrackingEvent trackingEvent);

    Task<int> GetTotalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    );
    Task<List<IntervalPageViewsDto>> GetIntervalPageViewsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    );

    Task<int> GetTotalVisitsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    );
    Task<List<IntervalVisitsDto>> GetIntervalVisitsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    );

    Task<int> GetTotalVisitorsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null
    );
    Task<List<IntervalVisitorsDto>> GetIntervalVisitorsAsync(
        int trackingCode,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        IntervalType? interval = IntervalType.Weeks
    );
}
