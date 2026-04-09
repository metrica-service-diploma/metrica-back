namespace metrica_back.src.Business.DTOs;

public class WebsiteResponseDto
{
    public Guid Id { get; set; }
    public string TrackingCode { get; set; }
    public string Name { get; set; }
    public string Domain { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateWebsiteRequestDto
{
    public string Name { get; set; }
    public string Domain { get; set; }
}
