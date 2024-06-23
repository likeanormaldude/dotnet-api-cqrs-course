using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Books;
using Restaurants.Application.Restaurants;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        // Services
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddScoped<IBooksService, BooksService>();

        services.AddAutoMapper(applicationAssembly);
        
        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();
    }
}
