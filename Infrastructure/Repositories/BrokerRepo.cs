using Domain.Entities.UsersEnities;
using Domain.Enums;
using Infrastructure.Interfaces;
using Infrastructure.Presistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BrokerRepo : IBrokerRepo
    {
        private readonly AppDbContext _context;

        public BrokerRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetBrokerBySlug(string slug, CancellationToken ct = default) 
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Slug == slug);
        }
    }
}
