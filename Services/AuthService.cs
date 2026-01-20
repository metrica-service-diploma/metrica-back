// using System.IdentityModel.Tokens.Jwt;
// using System.Security.Claims;
// using System.Text;
// using metrica_back.Data;
// using metrica_back.Models;
// using Microsoft.EntityFrameworkCore;
// using Microsoft.IdentityModel.Tokens;

// namespace metrica_back.Services
// {
//     public interface IAuthService
//     {
//         string GenerateJwtToken(User user);
//         Task<User?> GetUserFromTokenAsync(string token);
//     }

//     public class AuthService(AppDbContext appDbContext, IConfiguration configuration) : IAuthService
//     {
//         public string GenerateJwtToken(User user)
//         {
//             SymmetricSecurityKey securityKey = new(
//                 Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])
//             );
//             ClaimsIdentity claimsIdentity = new(
//                 [
//                     new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//                     new Claim(ClaimTypes.Name, user.UserName),
//                     new Claim(ClaimTypes.Email, user.Email),
//                 ]
//             );
//             JwtSecurityTokenHandler tokenHandler = new();

//             return tokenHandler.WriteToken(
//                 tokenHandler.CreateToken(
//                     new SecurityTokenDescriptor
//                     {
//                         Subject = claimsIdentity,
//                         Expires = DateTime.UtcNow.AddHours(24),
//                         SigningCredentials = new SigningCredentials(
//                             securityKey,
//                             SecurityAlgorithms.HmacSha256Signature
//                         ),
//                         Issuer = configuration["Jwt:Issuer"],
//                         Audience = configuration["Jwt:Audience"],
//                     }
//                 )
//             );
//         }

//         public async Task<User?> GetUserFromTokenAsync(string token)
//         {
//             try
//             {
//                 SymmetricSecurityKey securityKey = new(
//                     Encoding.ASCII.GetBytes(configuration["Jwt:Secret"])
//                 );
//                 var claimsPrincipal = new JwtSecurityTokenHandler().ValidateToken(
//                     token,
//                     new TokenValidationParameters
//                     {
//                         ValidateIssuerSigningKey = true,
//                         IssuerSigningKey = securityKey,
//                         ValidateIssuer = true,
//                         ValidIssuer = configuration["Jwt:Issuer"],
//                         ValidateAudience = true,
//                         ValidAudience = configuration["Jwt:Audience"],
//                         ValidateLifetime = true,
//                         ClockSkew = TimeSpan.Zero,
//                     },
//                     out _
//                 );

//                 if (
//                     Guid.TryParse(
//                         claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
//                         out Guid userId
//                     )
//                 )
//                     return await appDbContext.Users.FirstOrDefaultAsync(user => user.Id == userId);
//             }
//             catch
//             {
//                 return null;
//             }

//             return null;
//         }
//     }
// }
