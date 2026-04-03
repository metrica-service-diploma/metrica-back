namespace metrica_back.src.Models;

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
