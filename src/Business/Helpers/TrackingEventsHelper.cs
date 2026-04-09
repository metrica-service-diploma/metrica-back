using metrica_back.src.Domain.Enums;

namespace metrica_back.src.Business.Helpers;

public static class TrackingEventsHelper
{
    public static (int intervalValue, string intervalType) GetIntervalParameters(
        IntervalType intervalType
    )
    {
        return intervalType switch
        {
            IntervalType.Days => (1, "DAY"),
            IntervalType.Weeks => (1, "WEEK"),
            IntervalType.Months => (1, "MONTH"),
            _ => (1, "WEEK"),
        };
    }
}
