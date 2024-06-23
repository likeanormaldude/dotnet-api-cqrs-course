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
        await dbContext.SaveChangesAsync();
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

        await dbContext.SaveChangesAsync();

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

        await dbContext.SaveChangesAsync();

        return await dbContext.Books.ToListAsync();
    }

    public async Task<int> CreateRestaurant(Restaurant restaurant)
    {
        dbContext.Restaurants.Add(restaurant);

        await dbContext.SaveChangesAsync();
        return restaurant.Id;
    }
}
