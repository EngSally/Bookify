using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class BookConfigration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(e =>  new { e.Title,e.AuthorId }).IsUnique();
            builder.Property(e => e.Title).HasMaxLength(100);
            builder.Property(e => e.Publisher).HasMaxLength(200);
            builder.Property(e => e.Hall).HasMaxLength(50);
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE();");
        }
    }

{