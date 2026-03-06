
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync(CancellationToken ct = default);

        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct = default);
    }
}
