using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using metrica_back.src.Business.Interfaces.Services;
using metrica_back.src.Domain.Models;
using Microsoft.IdentityModel.Tokens;

namespace metrica_back.src.External.Services;

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor,
    IConfiguration configuration
) : ICurrentUserService
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
