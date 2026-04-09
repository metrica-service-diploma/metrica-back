using metrica_back.src.Business.Interfaces.Repositories;
using metrica_back.src.Domain.Models;
using metrica_back.src.External.Databases.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External.Repositories;

public class UserRepository(PostgreSqlContext postgreSqlContext) : IUserRepository
{
    public async Task<User> CreateAsync(User user)
    {
        await postgreSqlContext.Users.AddAsync(user);
        await postgreSqlContext.SaveChangesAsync();

        return user;
    }

    public async Task<bool> ExistsByUserNameOrEmailAsync(string userName, string email)
    {
        return await postgreSqlContext.Users.AnyAsync(u =>
            u.UserName == userName || u.Email == email
        );
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await postgreSqlContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}
