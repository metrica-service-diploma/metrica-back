using System.Security.Claims;

namespace metrica_back.Services
{
    public interface IUserService
    {
        Guid? GetCurrentUserId();
    }

    public class UserService(IHttpContextAccessor httpContextAccessor) : IUserService
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
    }
}
