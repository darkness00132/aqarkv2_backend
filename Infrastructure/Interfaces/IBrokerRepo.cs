
using Domain.Identity;

namespace Infrastructure.Interfaces
{
    public interface IBrokerRepo
    {
        Task<User?> GetBrokerBySlug(string slug, CancellationToken ct = default);
    }
}
