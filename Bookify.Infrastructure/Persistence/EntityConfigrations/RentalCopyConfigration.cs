using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class RentalCopyConfigration : IEntityTypeConfiguration<RentalCopy>
    {
        public void Configure(EntityTypeBuilder<RentalCopy> builder)
        {
            builder.HasKey(k => new { k.RentalId, k.BookCopyId });
            builder.Property(e => e.RentalDate).HasDefaultValueSql("CAST(GETDATE() AS Date)");
          
            builder.HasQueryFilter(r => !r.Rental!.Deleted);

        }
        }
}
