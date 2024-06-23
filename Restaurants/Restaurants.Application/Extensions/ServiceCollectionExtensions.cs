using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Books;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddScoped<IBooksService, BooksService>();

        services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);
    }
}
