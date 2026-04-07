namespace metrica_back.src.Core.Models;

public class User
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Website> Websites { get; set; }
}

public class Website
{
    public Guid Id { get; set; }
    public int TrackingCode { get; set; }
    public string Name { get; set; }
    public string Domain { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
}

public class TrackingEvent
{
    public Guid Id { get; set; }
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
