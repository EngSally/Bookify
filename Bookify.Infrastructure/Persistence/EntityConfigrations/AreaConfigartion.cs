using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class AreaConfigartion : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> builder)
        {
            builder.HasIndex(e => new { e.Name, e.GovernorateId }).IsUnique();
            builder.Property(e => e.Name).HasMaxLength(100);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE();");
        }
    }
}
