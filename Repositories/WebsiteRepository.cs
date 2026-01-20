using metrica_back.Data;
using metrica_back.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.Repositories
{
    public interface IWebsiteRepository
    {
        public Task<List<Website>> GetUserWebsitesAsync(Guid userId);
        public Task<Website?> GetWebsiteByIdAsync(Guid id);
        public Task<bool> IsWebsiteExistsAsync(string name, string domain);
        public Task<Website> CreateWebsiteAsync(Website website);
        public Task<int> GetTrackingCode();
    }

    public class WebsiteRepository(AppDbContext appDbContext) : IWebsiteRepository
    {
        public async Task<List<Website>> GetUserWebsitesAsync(Guid userId) =>
            await appDbContext.Websites.Where(website => website.UserId == userId).ToListAsync();

        public async Task<Website?> GetWebsiteByIdAsync(Guid id) =>
            await appDbContext.Websites.FirstOrDefaultAsync(website => website.Id == id);

        public async Task<bool> IsWebsiteExistsAsync(string name, string domain) =>
            await appDbContext.Websites.FirstOrDefaultAsync(website =>
                website.Name == name && website.Domain == domain
            ) != null;

        public async Task<Website> CreateWebsiteAsync(Website website)
        {
            await appDbContext.Websites.AddAsync(website);
            await appDbContext.SaveChangesAsync();

            return website;
        }

        public async Task<int> GetTrackingCode() => appDbContext.Websites.Count() + 1;
    }
}
