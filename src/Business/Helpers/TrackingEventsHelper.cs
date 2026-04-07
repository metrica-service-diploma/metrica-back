using metrica_back.src.Domain.Enums;

namespace metrica_back.src.Business.Helpers;

public static class TrackingEventsHelper
{
    public static (int intervalValue, string intervalType) GetIntervalParameters(Interval interval)
    {
        return interval switch
        {
            Interval.Days => (1, "DAY"),
            Interval.Weeks => (1, "WEEK"),
            Interval.Months => (1, "MONTH"),
            _ => (1, "WEEK"),
        };
    }
}
