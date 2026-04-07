using metrica_back.src.Core.Models;

namespace metrica_back.src.Core.Interfaces.Services;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
    string GetJwtSecurityToken(User user);
}
