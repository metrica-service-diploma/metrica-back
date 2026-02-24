namespace metrica_back.Models
{
    public class TrackingEvent
    {
        public Guid Id { get; set; }
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
}
