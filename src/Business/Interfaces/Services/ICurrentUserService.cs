using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Interfaces.Services;

public interface ICurrentUserService
{
    Guid? GetCurrentUserId();
    string GetJwtSecurityToken(User user);
}
