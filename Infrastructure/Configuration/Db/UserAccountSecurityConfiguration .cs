using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Db
{
    public class UserAccountSecurityConfiguration : IEntityTypeConfiguration<UserAccountSecurity>
    {
        public void Configure(EntityTypeBuilder<UserAccountSecurity> builder)
        {
            builder.HasOne(s => s.User)
                .WithOne(u => u.Security)
                .HasForeignKey<UserAccountSecurity>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
