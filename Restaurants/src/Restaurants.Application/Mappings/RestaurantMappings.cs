using Mapster;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Mappings;

public class RestaurantMappings : IRegisterMapsterConfig
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<Restaurant, RestaurantDto>()
            .Map(dest => dest.City, src => src.Address != null ? src.Address.City : "")
            .Map(dest => dest.Street, src => src.Address != null ? src.Address.Street : "")
            .Map(dest => dest.PostalCode, src => src.Address != null ? src.Address.PostalCode : "")
            .Map(dest => dest.Dishes, src => src.Dishes)
            .Map(dest => dest.Owner, src => src.Owner.UserName)
            .Map(dest => dest.LogoSasUrl, src => src.LogoUrl ?? string.Empty);

        config
            .NewConfig<CreateRestaurantCommand, Restaurant>()
            .Map(dest => dest.Address, src => MapToAddress(src))
            .Map(dest => dest.Dishes, src => src.Dishes);

        config
            .NewConfig<UpdateRestaurantCommand, Restaurant>()
            .Map(dest => dest.Address, src => MapToAddress(src))
            .Map(dest => dest.Dishes, src => src.Dishes)
            .Ignore(dest => dest.Owner);
    }

    private static Address MapToAddress(CreateRestaurantCommand src) =>
        new Address
        {
            City = src.City,
            Street = src.Street,
            PostalCode = src.PostalCode,
        };

    private static Address MapToAddress(UpdateRestaurantCommand src) =>
        new Address
        {
            City = src.City,
            Street = src.Street,
            PostalCode = src.PostalCode,
        };
}
