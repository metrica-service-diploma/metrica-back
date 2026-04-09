using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsByUserNameOrEmailAsync(string userName, string email);
    Task<User> CreateAsync(User user);
}
