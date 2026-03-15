using Application.DTOs.Ad;

namespace Application.Interfaces
{
    public interface IAdService
    {
        Task<List<AdListItemDTO>> GetAllAds(CancellationToken ct = default);

        Task<AdDTO> GetAdBySlug(string slug, CancellationToken ct = default);

        Task<AdDTO> GetAdById(Guid id, CancellationToken ct = default);

        Task CreateAdAsync(CreateAdDTO ad, Guid userId , CancellationToken ct = default);

        Task UpdateAdAsync(UpdateAdDTO ad, CancellationToken ct = default);

        Task DeleteAdAsync(Guid id, CancellationToken ct = default);
    }
}
