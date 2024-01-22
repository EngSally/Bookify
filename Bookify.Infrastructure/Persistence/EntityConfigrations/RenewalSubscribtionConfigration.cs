using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookify.Infrastructure.Persistence.EntityConfigrations
{
    internal class RenewalSubscribtionConfigration : IEntityTypeConfiguration<RenewalSubscribtion>
    {
        public void Configure(EntityTypeBuilder<RenewalSubscribtion> builder)
        {
            builder.Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE() ");
        }
    }
}
