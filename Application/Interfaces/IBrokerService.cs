using Application.DTOs.Brokers;
using Application.DTOs.User;

namespace Application.Interfaces
{
    public interface IBrokerService
    {
        Task<List<BrokerItemList>> GetAllBrokers(CancellationToken ct = default);

        Task<Broker> GetBrokerBySlug(string slug, CancellationToken ct = default);

        Task<Broker> GetMyBrokerProfile(Guid brokerId, CancellationToken ct = default);

        Task UpdateBrokerBrofile(Guid brokerId, UpdateBrokerProfile dto);
    }
}
