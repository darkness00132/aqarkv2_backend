
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Interfaces
{
    public interface IUnitOfWork
    {
        public Task SaveChangesAsync();

        public Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
