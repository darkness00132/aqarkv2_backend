using Domain.Entities.AdEntities;

namespace Infrastructure.Interfaces.Ads
{
    public interface IAdLogRepo
    {
        Task LogAsync(AdLog log, CancellationToken ct = default);
        Task<List<AdLog>> GetByAdId(Guid adId, CancellationToken ct = default);
    }
}
