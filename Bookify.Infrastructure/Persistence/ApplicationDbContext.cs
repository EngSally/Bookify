
using Bookify.Infrastructure.Persistence.EntityConfigrations;
using System.Reflection;

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
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			
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