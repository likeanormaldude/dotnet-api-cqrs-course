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
        var restaurants = await dbContext.Restaurants
            .Include(r => r.Dishes)
            .ToListAsync();
        return restaurants;
    }
    public Task<Restaurant?> GetRestaurantByIdAsync(int id)
    {
        var restaurant = dbContext.Restaurants
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync( x => x.Id == id);

        return restaurant;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        var books = await dbContext.Books.ToListAsync();
        return books;
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        var books = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

        if(books == null)
            return null;

        return books;
    }

    public async Task<IEnumerable<Book>> CreateBook(Book book)
    {
        dbContext.Books.Add(book);
        await SaveChanges();
        return await dbContext.Books.ToListAsync();
    }

    public async Task<IEnumerable<Book>?> DeleteBook(int id)
    {
        var foundBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);

        if (foundBook is null)
        {
            return null;
        }

        dbContext.Remove(foundBook);

        await SaveChanges();

        return await dbContext.Books.ToListAsync();
    }

    public async Task<IEnumerable<Book>?> UpdateBook(Book newBook)
    {
        var foundBook = await dbContext.Books.FirstOrDefaultAsync(x => x.Id == newBook.Id);

        if (foundBook == null)
            return null;

        foundBook.Title = newBook.Title;
        foundBook.Description = newBook.Description;
        foundBook.NumberOfPages = newBook.NumberOfPages;

        await SaveChanges();

        return await dbContext.Books.ToListAsync();
    }

    public async Task<int> CreateRestaurant(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);

        await SaveChanges();
        return restaurant.Id;
    }

    public async Task DeleteRestaurant(Restaurant restaurant)
    {
        dbContext.Remove(restaurant);

        await SaveChanges();
    }

    public async Task<IEnumerable<Restaurant>?> UpdateRestaurant(Restaurant newRestaurant)
    {
        var found = await dbContext.Restaurants.FirstOrDefaultAsync(x => x.Id == newRestaurant.Id);

        if (found == null)
            return null;

        found.Name = newRestaurant.Name;
        found.Description = newRestaurant.Description;
        found.Category = newRestaurant.Category;
        found.HasDelivery = newRestaurant.HasDelivery;

        await SaveChanges();

        return await dbContext.Restaurants.ToListAsync();
    }

    public Task SaveChanges() => dbContext.SaveChangesAsync();
}
