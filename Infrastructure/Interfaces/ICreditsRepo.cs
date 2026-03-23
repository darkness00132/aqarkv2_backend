
namespace Infrastructure.Interfaces
{
    public interface ICreditsRepo
    {
        Task<bool> DeductAsync(Guid userId, int cost,CancellationToken ct=default);
    }
}
