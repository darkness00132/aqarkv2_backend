using Domain.Entities.UsersEnities;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;
using Application.Interfaces.Users;

namespace Infrastructure.Repositories.Users
{
    public class UserRepo : IUserRepo
    {
        private readonly AppDbContext _context;
        public UserRepo(AppDbContext context) => _context = context;

        public async Task<User?> GetByIdWithBrokerProfileAsync(Guid userId)
            => await _context.Users
            .Include(u => u.BrokerProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);
    }
}
