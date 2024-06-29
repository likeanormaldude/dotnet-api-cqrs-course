using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories;

public interface IRestaurantsRepository
{
    // Restaurants
    Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    Task<Restaurant?> GetRestaurantByIdAsync(int id);
    Task<int> CreateRestaurant(Restaurant restaurant);
    Task DeleteRestaurant(Restaurant restaurant);
    Task<IEnumerable<Restaurant>?> UpdateRestaurant(Restaurant restaurant);
    Task SaveChanges();


    // Books
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<IEnumerable<Book>> CreateBook(Book book);
    Task<IEnumerable<Book>?> DeleteBook(int id);
    Task<IEnumerable<Book>?> UpdateBook(Book newBook);

}
