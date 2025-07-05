using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger)
    : IRestaurantsService
{
    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        IEnumerable<Restaurant> restaurants = await restaurantsRepository.GetAllAsync();
        IEnumerable<RestaurantDto> restaurantDtos = restaurants.Select(RestaurantDto.FromEntity)!;
        return restaurantDtos;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation($"Getting restaurants by id {id}");
        Restaurant? restaurant = await restaurantsRepository.GetByIdAsync(id);
        RestaurantDto? restaurantDto = RestaurantDto.FromEntity(restaurant);
        return restaurantDto;
    }
}
