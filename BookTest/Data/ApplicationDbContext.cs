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
            builder.Entity<BookCategory>().HasKey(k => new { k.CategoryId, k.BookId });

            builder.Entity<Category>().Property(e => e.CretedOn).HasDefaultValueSql("GETDATE()");
            base.OnModelCreating(builder);
        }



        
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookCategory> BooksCategories { get; set; }
        public DbSet<Category> Categories { get; set; }



    }
}