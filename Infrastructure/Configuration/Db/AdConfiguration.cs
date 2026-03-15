using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configuration.Db
{
    public class AdConfiguration : IEntityTypeConfiguration<Ad>
    {
        public void Configure(EntityTypeBuilder<Ad> builder)
        {
            builder.Property(a => a.Title)
                    .IsRequired()
                    .HasMaxLength(255);
            builder.Property(a => a.Slug)
                    .IsRequired()
                    .HasMaxLength(255);

            builder.HasIndex(a => a.Slug)
                    .IsUnique();
        }
    }
}
