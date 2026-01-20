namespace metrica_back.Dto
{
    public class WebsiteResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Domain { get; set; }
        public string TrackingCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public UserResponseDto User { get; set; }
    }

    public class CreateWebsiteRequestDto
    {
        public string Name { get; set; }
        public string Domain { get; set; }
    }
}
