using metrica_back.Data;
using metrica_back.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUserByEmailAsync(string email);
        public Task<bool> IsUserExistsAsync(string userName, string email);
        public Task<User> CreateUserAsync(User user);
    }

    public class UserRepository(AppDbContext appDbContext) : IUserRepository
    {
        public async Task<User?> GetUserByEmailAsync(string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

        public async Task<bool> IsUserExistsAsync(string userName, string email) =>
            await appDbContext.Users.FirstOrDefaultAsync(user =>
                user.UserName == userName || user.Email == email
            ) != null;

        public async Task<User> CreateUserAsync(User user)
        {
            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            return user;
        }
    }
}
