using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using metrica_back.Models;
using Microsoft.IdentityModel.Tokens;

namespace metrica_back.Services
{
    public class JwtTokenService(IConfiguration configuration)
    {
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
}
