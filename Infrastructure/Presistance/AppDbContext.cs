using Domain.Entities;
using Domain.Entities;
using Domain.Entities.Identity;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Presistance
{
    public class AppDbContext : IdentityDbContext<User, Role, Guid>
    {
        public AppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<UserAccountSecurity> UserAccountSecurities { get; set; }

        public DbSet<Ad> Ads { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<AdLog> AdLogs { get; set; }

        public DbSet<CreditsPlan> CreditsPlans { get; set; }
        public DbSet<CreditsLog> CreditsLogs { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<PaymentAttempt> PaymentAttempts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
