using Restaurants.Application.Dtos;

namespace Restaurants.Application.Restaurants;

public interface IRestaurantsService
{
    Task<IEnumerable<RestaurantDto>> GetAllRestaurants();
    Task<RestaurantDto?> GetById(int id);
}