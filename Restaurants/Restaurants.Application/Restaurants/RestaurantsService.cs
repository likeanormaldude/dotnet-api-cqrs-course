using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

public class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger
) : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");

        var restaurants = await restaurantsRepository.GetAllRestaurantsAsync();
        var restaurantsDto = restaurants.Select(r => RestaurantDto.FromEntity(r));

        return restaurantsDto!;
    }

    public async Task<RestaurantDto?> GetById(int id)
    {
        logger.LogInformation($"Getting restaurant with Id {id}");
        var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(id);

        if (restaurant == null)
        {
            return null;
        }

        var restaurantDto = RestaurantDto.FromEntity(restaurant);
        return restaurantDto;
    }
}
