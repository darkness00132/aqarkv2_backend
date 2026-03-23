using Application.DTOs.Ad;
using Application.DTOs.Ad.Private;
using Domain.Entities.AdEntities;
using Shared.Filters;
using Shared.Pagination;

namespace Application.Interfaces
{
    public interface IAdService
    {
        Task<PaginationResult<List<AdListItemDTO>>> GetAllAds(AdFilters? filters, Pagination? pagination, CancellationToken ct = default);

        Task<PaginationResult<List<AdPrivateListItemDTO>>> GetMyAds(AdFilters? filters, Pagination? pagination,Guid userId,CancellationToken ct = default);

        Task<AdDTO> GetAdBySlug(string slug, CancellationToken ct = default);

        Task<Ad> GetAdById(Guid id, CancellationToken ct = default);

        Task CreateAdAsync(CreateAdDTO ad, Guid userId);

        Task UpdateAdAsync(Guid id, Guid userId ,UpdateAdDTO dto);

        Task DeleteAdAsync(Guid id, Guid userId);
    }
}
