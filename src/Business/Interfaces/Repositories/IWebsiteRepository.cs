using metrica_back.src.Domain.Models;

namespace metrica_back.src.Business.Interfaces.Repositories;

public interface IWebsiteRepository
{
    Task<IReadOnlyList<Website>> GetByUserIdAsync(Guid userId);
    Task<Website?> GetByIdAsync(Guid id);
    Task<bool> ExistsByNameOrDomainAsync(string name, string domain);
    Task<Website> CreateAsync(Website website);
    Task<int> GetNextTrackingCodeAsync();
}
