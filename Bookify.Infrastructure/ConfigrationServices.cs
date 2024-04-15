
using Bookify.Infrastructure.Repositories;
using Bookify.Infrastructure.Services;
using Bookify.Infrastructure.Services.BookCopies;
using Bookify.Infrastructure.Services.Rentals;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Bookify.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
        services.AddScoped<IAuthorsService, AuthorsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<IBooksService, BooksService>();
        services.AddScoped<IBookCopiesService, BookCopiesService>();
        services.AddScoped<IRentalService, RentalService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}