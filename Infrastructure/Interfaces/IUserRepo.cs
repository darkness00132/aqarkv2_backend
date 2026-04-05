using Domain.Entities.UsersEnities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepo
    {
        Task<User?> GetByIdWithBrokerProfileAsync(Guid userId);
    }
}
