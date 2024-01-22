using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations;

internal class CategoryConfigration:IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.HasIndex(e => e.Name).IsUnique();
       
        builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.Name).HasMaxLength(100);
    }
}