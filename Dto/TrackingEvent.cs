namespace metrica_back.Dto
{
    public class TrackingEventDto
    {
        public Guid SessionId { get; set; }
        public Guid WebsiteId { get; set; }
        public string EventType { get; set; }
        public DateTime Timestamp { get; set; }
        public string PageUrl { get; set; }
        public string PageTitle { get; set; }
        public string Referrer { get; set; }
        public string UserAgent { get; set; }
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }
        public string BrowserLanguage { get; set; }
    }

    public enum IntervalType
    {
        Days,
        Weeks,
        Months,
    }

    public class TotalPageViews
    {
        public int PageViews { get; set; }
    }

    public class IntervalPageViews
    {
        public int PageViews { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class PageViewsResponseDto
    {
        public Guid WebsiteId { get; set; }
        public int TotalPageViews { get; set; }
        public List<IntervalPageViews> IntervalPageViews { get; set; }
    }
}
