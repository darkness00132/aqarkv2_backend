using Domain.Entities.Brokers;

namespace Infrastructure.Interfaces.Brokers
{
    public interface IBrokerReviewRepo
    {
        Task<BrokerReview?> GetByIdForMutationAsync(int id);
        Task<BrokerReview?> GetByIdAsync(int id);
        Task<List<BrokerReview>> GetByBrokerSlugAsync(string slug);
        Task<List<BrokerReview>> GetByUserIdAsync(Guid userId);
        Task<BrokerReview?> GetByBrokerAndUserAsync(Guid brokerUserId, Guid userId);
        Task AddAsync(BrokerReview review);
        Task DeleteAsync(int id);
    }
}
