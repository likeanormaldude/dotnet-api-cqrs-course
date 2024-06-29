using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Books;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;

        var serviceProvider = services.BuildServiceProvider();

        // Any class could be passed to "ILogger" Here.
        // @see https://stackoverflow.com/a/70610564/7658732 for error "Some services are not able to be constructed..."
        var logger = serviceProvider.GetService<ILogger<RestaurantDto>>();

        if(logger != null)
            services.AddSingleton(typeof(ILogger), logger);

        // Services
        services.AddScoped<IBooksService, BooksService>();

        services.AddAutoMapper(applicationAssembly);
        
        services.AddValidatorsFromAssembly(applicationAssembly)
            .AddFluentValidationAutoValidation();

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
    }
}
