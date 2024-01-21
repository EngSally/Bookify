
namespace Bookify.Infrastructure.Persistence
{
	public class ApplicationDbContext : IdentityDbContext<ApplicationUser> , IApplicationDbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder builder)
		{
			//sequance
			builder.HasSequence<int>("SerialNumberSequance", "Shared")
				.StartsAt(10000);
			builder.Entity<BookCopy>()
				.Property(e => e.SerialNumber)
				.HasDefaultValueSql("Next  Value For Shared.SerialNumberSequance");

			builder.Entity<BookCategory>().HasKey(k => new { k.CategoryId, k.BookId });
			builder.Entity<RentalCopy>().HasKey(k => new { k.RentalId, k.BookCopyId });
			builder.Entity<Category>().Property(e => e.CreatedOn).HasDefaultValueSql("GETDATE()");
			builder.Entity<Rental>().HasQueryFilter(r => !r.Deleted);
			builder.Entity<RentalCopy>().HasQueryFilter(r => !r.Rental!.Deleted);
			base.OnModelCreating(builder);
		}

		public DbSet<Area> Areas { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<BookCategory> BooksCategories { get; set; }
		public DbSet<BookCopy> BooksCopies { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<Governorate> Governorates { get; set; }
		public DbSet<Rental> Rentals { get; set; }
		public DbSet<RentalCopy> RentalCopies { get; set; }

		public DbSet<Subscriber> Subscribers { get; set; }
		public DbSet<RenewalSubscribtion> RenewalSubscribtions { get; set; }




	}
}