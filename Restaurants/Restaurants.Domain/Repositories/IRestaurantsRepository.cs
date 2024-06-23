using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);

    Task<IEnumerable<Book>> GetAllBooksAsync();
    //Task<Book?> GetBooksByIdAsync(int id);


}
