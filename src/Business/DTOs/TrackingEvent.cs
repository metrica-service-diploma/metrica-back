namespace metrica_back.src.Business.DTOs;

public class CreateTrackingEventRequestDto
{
    public Guid ClientId { get; set; }
    public Guid SessionId { get; set; }
    public int TrackingCode { get; set; }
    public DateTime CreatedAt { get; set; }
    public string PageUrl { get; set; }
    public string PageTitle { get; set; }
    public string Referrer { get; set; }
    public string UserAgent { get; set; }
    public int ScreenWidth { get; set; }
    public int ScreenHeight { get; set; }
    public string BrowserLanguage { get; set; }
}

public class IntervalPageViewsDto
{
    public int PageViews { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class PageViewsResponseDto
{
    public int TotalPageViews { get; set; }
    public List<IntervalPageViewsDto> IntervalPageViews { get; set; }
}

public class IntervalVisitsDto
{
    public int Visits { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class VisitsResponseDto
{
    public int TotalVisits { get; set; }
    public List<IntervalVisitsDto> IntervalVisits { get; set; }
}

public class IntervalVisitorsDto
{
    public int Visitors { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class VisitorsResponseDto
{
    public int TotalVisitors { get; set; }
    public List<IntervalVisitorsDto> IntervalVisitors { get; set; }
}
