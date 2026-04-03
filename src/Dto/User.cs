namespace metrica_back.src.Dto;

public class UserResponseDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SignUpRequestDto
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignInRequestDto
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class SignInResponseDto
{
    public UserResponseDto User { get; set; }
    public string AccessToken { get; set; }
}
