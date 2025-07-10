using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IRestaurantsService, RestaurantsService>();
        services.AddSingleton(GetMappingConfigs());
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddValidatorsFromAssemblyContaining(typeof(RestaurantsService));
    }

    private static TypeAdapterConfig GetMappingConfigs()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Dish, DishDto>();
        config
            .NewConfig<Restaurant, RestaurantDto>()
            .Map(dest => dest.City, src => src.Address != null ? src.Address.City : "")
            .Map(dest => dest.Street, src => src.Address != null ? src.Address.Street : "")
            .Map(dest => dest.PostalCode, src => src.Address != null ? src.Address.PostalCode : "")
            .Map(dest => dest.Dishes, src => src.Dishes);

        config
            .NewConfig<CreateRestaurantDto, Restaurant>()
            .Map(dest => dest.Address.City, src => src.City)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.PostalCode, src => src.PostalCode)
            .Map(dest => dest.Dishes, src => src.Dishes);

        return config;
    }
}
