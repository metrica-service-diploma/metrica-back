using metrica_back.src.Data;
using metrica_back.src.Models;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.Repositories;

public interface IWebsiteRepository
{
    public Task<List<Website>> GetUserWebsitesAsync(Guid userId);
    public Task<Website?> GetWebsiteByIdAsync(Guid id);
    public Task<bool> IsWebsiteExistsAsync(string name, string domain);
    public Task<Website> CreateWebsiteAsync(Website website);
    public Task<int> GetTrackingCode();
}

public class WebsiteRepository(PostgreSQLContext postgreSQLContext) : IWebsiteRepository
{
    public async Task<List<Website>> GetUserWebsitesAsync(Guid userId) =>
        await postgreSQLContext
            .Websites.Where(website => website.UserId == userId)
            .OrderBy(website => website.TrackingCode)
            .ToListAsync();

    public async Task<Website?> GetWebsiteByIdAsync(Guid id) =>
        await postgreSQLContext.Websites.FirstOrDefaultAsync(website => website.Id == id);

    public async Task<bool> IsWebsiteExistsAsync(string name, string domain) =>
        await postgreSQLContext.Websites.FirstOrDefaultAsync(website =>
            website.Name == name && website.Domain == domain
        ) != null;

    public async Task<Website> CreateWebsiteAsync(Website website)
    {
        await postgreSQLContext.Websites.AddAsync(website);
        await postgreSQLContext.SaveChangesAsync();

        return website;
    }

    public async Task<int> GetTrackingCode() => postgreSQLContext.Websites.Count() + 1;
}
