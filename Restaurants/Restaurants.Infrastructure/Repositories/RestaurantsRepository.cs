using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext)
    : IRestaurantsRepository
{
    public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
    {
        var restaurants = await dbContext.Restaurants.ToListAsync();
        return restaurants;
    }
    public Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = dbContext.Restaurants.FirstOrDefaultAsync( x => x.Id == id);
        return restaurant;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        var books = await dbContext.Books.ToListAsync();
        return books;
    }

}
