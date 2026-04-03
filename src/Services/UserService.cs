using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using metrica_back.src.Models;
using Microsoft.IdentityModel.Tokens;

namespace metrica_back.src.Services;

public interface IUserService
{
    Guid? GetCurrentUserId();
    string GetJwtSecurityToken(User user);
}

public class UserService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    : IUserService
{
    public Guid? GetCurrentUserId()
    {
        string? userIdClaim = httpContextAccessor
            .HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)
            ?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
            return null;

        if (Guid.TryParse(userIdClaim, out Guid userId))
            return userId;

        return null;
    }

    public string GetJwtSecurityToken(User user)
    {
        SymmetricSecurityKey securityKey = new(
            Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])
        );
        ClaimsIdentity claimsIdentity = new(
            [
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
            ]
        );
        JwtSecurityTokenHandler tokenHandler = new();

        return tokenHandler.WriteToken(
            tokenHandler.CreateToken(
                new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = DateTime.UtcNow.AddHours(24),
                    SigningCredentials = new SigningCredentials(
                        securityKey,
                        SecurityAlgorithms.HmacSha256Signature
                    ),
                    Issuer = configuration["Jwt:Issuer"],
                    Audience = configuration["Jwt:Audience"],
                }
            )
        );
    }
}
