using metrica_back.Dto;

namespace metrica_back.Helpers
{
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
}
