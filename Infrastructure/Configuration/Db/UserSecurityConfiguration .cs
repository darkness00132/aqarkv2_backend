using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Db
{
    public class UserSecurityConfiguration : IEntityTypeConfiguration<UserSecurity>
    {
        public void Configure(EntityTypeBuilder<UserSecurity> builder)
        {
            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.User)
                .WithOne(u => u.Security)
                .HasForeignKey<UserSecurity>(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.LastLoginIp)
                .HasMaxLength(45); // covers IPv6

            builder.Property(s => s.BlockReason)
                .HasMaxLength(500);
        }
    }
}
