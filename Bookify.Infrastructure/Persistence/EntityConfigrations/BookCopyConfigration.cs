using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class BookCopyConfigration : IEntityTypeConfiguration<BookCopy>

    {
        public void Configure(EntityTypeBuilder<BookCopy> builder)
        {
            builder.Property(e => e.SerialNumber)
                .HasDefaultValueSql("Next  Value For Shared.SerialNumberSequance");
        }
    }
}
