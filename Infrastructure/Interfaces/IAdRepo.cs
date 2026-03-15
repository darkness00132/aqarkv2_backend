using Domain.Entities;

namespace Infrastructure.Interfaces
{
    public interface IAdRepo
    {
        Task<List<Ad>> GetAllAsync(CancellationToken ct = default);

        Task<Ad?> GetAdBySlugAsync(string slug, CancellationToken ct = default);

        Task<Ad?> GetAdByIdAsync(Guid id, CancellationToken ct = default);

        Task<Guid> CreateAdAsync(Ad ad , CancellationToken ct=default);
    }
}
