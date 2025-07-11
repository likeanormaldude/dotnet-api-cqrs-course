using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        //Assembly applicationAssembly = typeof(ServiceCollectionExtensions).Assembly;
        Type applicationType = typeof(ServiceCollectionExtensions);

        services.AddSingleton(GetMappingConfigs());
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddValidatorsFromAssemblyContaining(applicationType);
        //services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly));
    }

    private static TypeAdapterConfig GetMappingConfigs()
    {
        var config = TypeAdapterConfig.GlobalSettings;

        config.NewConfig<Dish, DishDto>();
        config.NewConfig<DishDto, Dish>();
        config
            .NewConfig<Restaurant, RestaurantDto>()
            .Map(dest => dest.City, src => src.Address != null ? src.Address.City : "")
            .Map(dest => dest.Street, src => src.Address != null ? src.Address.Street : "")
            .Map(dest => dest.PostalCode, src => src.Address != null ? src.Address.PostalCode : "")
            .Map(dest => dest.Dishes, src => src.Dishes);

        config
            .NewConfig<CreateRestaurantCommand, Restaurant>()
            .Map(dest => dest.Address.City, src => src.City)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.PostalCode, src => src.PostalCode)
            .Map(dest => dest.Dishes, src => src.Dishes);

        config
            .NewConfig<UpdateRestaurantCommand, Restaurant>()
            .Map(dest => dest.Address.City, src => src.City)
            .Map(dest => dest.Address.Street, src => src.Street)
            .Map(dest => dest.Address.PostalCode, src => src.PostalCode)
            .Map(dest => dest.Dishes, src => src.Dishes);

        return config;
    }
}
