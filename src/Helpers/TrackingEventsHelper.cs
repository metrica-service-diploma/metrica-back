using metrica_back.src.Dto;

namespace metrica_back.src.Helpers;

public static class TrackingEventsHelper
{
    public static (int intervalValue, string intervalType) GetIntervalParameters(
        IntervalType interval
    )
    {
        return interval switch
        {
            IntervalType.Days => (1, "DAY"),
            IntervalType.Weeks => (1, "WEEK"),
            IntervalType.Months => (1, "MONTH"),
            _ => (1, "WEEK"),
        };
    }
}
