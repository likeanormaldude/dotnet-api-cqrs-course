using MapsterMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants;

internal class RestaurantsService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<RestaurantsService> logger,
    IMapper mapper
) : IRestaurantsService
{
    public async Task<int> Create(CreateRestaurantDto dto)
    {
        logger.LogInformation("Creating a new restaurant");
        var restaurant = mapper.Map<Restaurant>(dto);
        int id = await restaurantsRepository.Create(restaurant);
        return id;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllRestaurants()
    {
        logger.LogInformation("Getting all restaurants");
        IEnumerable<Restaurant> restaurants = await restaurantsRepository.GetAllAsync();
        IEnumerable<RestaurantDto> restaurantDtos = mapper.Map<IEnumerable<Restaurant>, IEnumerable<RestaurantDto>>(
            restaurants
        );
        return restaurantDtos;
    }

    public async Task<RestaurantDto?> GetRestaurantById(int id)
    {
        logger.LogInformation($"Getting restaurants by id {id}");
        Restaurant? restaurant = await restaurantsRepository.GetByIdAsync(id);
        RestaurantDto? restaurantDto = mapper.Map<RestaurantDto?>(restaurant ?? new object());
        return restaurantDto;
    }
}
