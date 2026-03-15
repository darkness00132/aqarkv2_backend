using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Presistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AdRepo : IAdRepo
    {
        private readonly AppDbContext _context;

        public AdRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ad?> GetAdBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _context.Ads
                        .AsNoTracking()
                        .Include(a => a.Images)
                        .Include(a => a.User)
                        .FirstOrDefaultAsync(a => a.Slug == slug, ct);
        }

        public async Task<Ad?> GetAdByIdAsync(Guid id, CancellationToken ct=default)
        {
            return await _context.Ads
                .AsNoTracking()
                .Include(a=>a.Images)
                .Include(a=>a.User)
                .FirstOrDefaultAsync(a=>a.Id == id,ct);
        }
        public async Task<List<Ad>> GetAllAsync(CancellationToken ct=default)
        {
            return await _context.Ads
                .AsNoTracking()
                .Include(p => p.Images.Take(1))
                .Include(a => a.User)
                .ToListAsync(ct);
        }

        public async Task<Guid> CreateAdAsync(Ad ad,CancellationToken ct=default)
        {
            ad.Id = Guid.NewGuid();
            ad.CreatedAt = DateTime.UtcNow;
            await _context.Ads.AddAsync(ad,ct);
            return ad.Id;
        }
    }
}
