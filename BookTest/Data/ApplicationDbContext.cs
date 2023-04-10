using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BookTest.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>().Property(e => e.CretedOn).HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(builder);
        }
        public DbSet<Category> categories { get; set; }
        public DbSet<Author> authors { get; set; }



    }
}