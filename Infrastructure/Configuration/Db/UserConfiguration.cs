using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Db
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u=>u.Credits).IsRequired().HasDefaultValue(0);
            builder.Property(u=>u.IsProfileCompleted).IsRequired().HasDefaultValue(false);
        }
    }
}
