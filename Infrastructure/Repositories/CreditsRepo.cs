using Infrastructure.Interfaces;
using Infrastructure.Presistance;

namespace Infrastructure.Repositories
{
    public class CreditsRepo : ICreditsRepo
    {
        private readonly AppDbContext _context;

        public CreditsRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeductAsync(Guid userId, int cost, CancellationToken ct=default)
        {
            var user = await _context.Users.FindAsync([userId],ct);
            if (user is null) return false;
            if (user.Credits < cost) return false;
            user.Credits -= cost;
            return true;
        }
    }
}
