using metrica_back.src.Data;
using metrica_back.src.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.Repositories;

public interface IUserRepository
{
    public Task<User?> GetUserByEmailAsync(string email);
    public Task<bool> IsUserExistsAsync(string userName, string email);
    public Task<User> CreateUserAsync(User user);
}

public class UserRepository(PostgreSQLContext postgreSQLContext) : IUserRepository
{
    public async Task<User?> GetUserByEmailAsync(string email) =>
        await postgreSQLContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task<bool> IsUserExistsAsync(string userName, string email) =>
        await postgreSQLContext.Users.FirstOrDefaultAsync(user =>
            user.UserName == userName || user.Email == email
        ) != null;

    public async Task<User> CreateUserAsync(User user)
    {
        await postgreSQLContext.Users.AddAsync(user);
        await postgreSQLContext.SaveChangesAsync();

        return user;
    }
}
