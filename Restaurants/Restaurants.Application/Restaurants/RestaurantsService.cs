using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(IRestaurantsRepository restaurantsRepository, ILogger<RestaurantsService> logger)
    : IRestaurantsService
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        IEnumerable<Restaurant> restaurants = await restaurantsRepository.GetAllAsync();
        return restaurants;
    }

    public async Task<Restaurant?> GetRestaurantById(int id)
    {
        logger.LogInformation($"Getting restaurants by id {id}");
        Restaurant? restaurants = await restaurantsRepository.GetByIdAsync(id);
        return restaurants;
    }
}
