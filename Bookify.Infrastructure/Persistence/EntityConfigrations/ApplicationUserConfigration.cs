

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations;

internal class ApplicationUserConfigration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(e => e.Email).IsUnique();
        builder.HasIndex(e => e.UserName).IsUnique();
        builder.Property(e => e.FullName).HasMaxLength(100);
        builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE();");
    }
}

