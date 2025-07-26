using Mapster;
using Restaurants.Application.Common;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Mappings;

public class DishMappings : IRegisterMapsterConfig
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Dish, DishDto>();
        config.NewConfig<CreateDishCommand, Dish>();
        config.NewConfig<DishDto, Dish>();
    }
}
