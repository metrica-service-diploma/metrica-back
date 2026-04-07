using metrica_back.src.Core.Interfaces.Repositories;
using metrica_back.src.Core.Models;
using metrica_back.src.External.Databases.PostgreSql;
using Microsoft.EntityFrameworkCore;

namespace metrica_back.src.External.Repositories;

public class WebsiteRepository(PostgreSqlContext postgreSqlContext) : IWebsiteRepository
{
    public async Task<Website> CreateAsync(Website website)
    {
        await postgreSqlContext.Websites.AddAsync(website);
        await postgreSqlContext.SaveChangesAsync();

        return website;
    }

    public async Task<bool> ExistsByNameOrDomainAsync(string name, string domain)
    {
        return await postgreSqlContext.Websites.AnyAsync(w => w.Name == name || w.Domain == domain);
    }

    public async Task<Website?> GetByIdAsync(Guid id)
    {
        return await postgreSqlContext.Websites.FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<IReadOnlyList<Website>> GetByUserIdAsync(Guid userId)
    {
        return await postgreSqlContext
            .Websites.Where(w => w.UserId == userId)
            .OrderBy(w => w.TrackingCode)
            .ToListAsync();
    }

    public async Task<int> GetNextTrackingCodeAsync()
    {
        var maxTrackingCode =
            await postgreSqlContext.Websites.MaxAsync(w => (int?)w.TrackingCode) ?? 0;

        return maxTrackingCode + 1;
    }
}
