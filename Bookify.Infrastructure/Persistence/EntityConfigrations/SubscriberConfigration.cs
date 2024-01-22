using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class SubscriberConfigration : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.HasIndex(e => e.Email).IsUnique();
            builder.HasIndex(e => e.NationalId).IsUnique();
            builder.HasIndex(e => e.MobilNum).IsUnique();
            builder.Property(e => e.FristName).HasMaxLength(100);
            builder.Property(e => e.LastName).HasMaxLength(100);
            builder.Property(e => e.NationalId).HasMaxLength(15);
            builder.Property(e => e.MobilNum).HasMaxLength(20);
            builder.Property(e => e.Email).HasMaxLength(150);
            builder.Property(e => e.ImageUrl).HasMaxLength(500);
            builder.Property(e => e.ImageUrlThumbnail).HasMaxLength(500);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE();");

        }
    }
}
