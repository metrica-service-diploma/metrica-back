using metrica_back.src.Core.Models;

namespace metrica_back.src.Core.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByUserNameOrEmailAsync(string userName, string email);
    Task<User> CreateAsync(User user);
}
