using Domain.Entities.UsersEnities;

namespace Infrastructure.Interfaces
{
    public interface IBrokerRepo
    {
        Task<User?> GetBrokerBySlug(string slug, CancellationToken ct = default);
    }
}
