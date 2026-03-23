using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IBrokerService
    {
        Task<List<BrokerItemList>> GetAllBrokers(CancellationToken ct);

        Task<Broker> GetBrokerBySlug(string slug, CancellationToken ct);
    }
}
