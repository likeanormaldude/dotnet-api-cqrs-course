using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

internal class RestaurantsRepository(RestaurantsDbContext dbContext) : IRestaurantsRepository
{
    public async Task<int> Create(Restaurant entity)
    {
        dbContext.Add(entity);
        await dbContext.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<IEnumerable<Restaurant>> GetAllAsync()
    {
        IEnumerable<Restaurant> restaurants = await dbContext
            .Restaurants.Include(x => x.Dishes)
            .Include(x => x.Owner)
            .ToListAsync();
        return restaurants;
    }

    public async Task<(IEnumerable<Restaurant>, int)> GetAllMatchingAsync(
        string? searchPhrase,
        int pageSize,
        int pageNumber,
        string? sortBy,
        SortDirection? sortDirection
    )
    {
        string searchPhraseLower = searchPhrase?.ToLower() ?? string.Empty;
        var baseQuery = dbContext
            .Restaurants.Include(x => x.Dishes)
            .Include(x => x.Owner)
            .Where(x =>
                string.IsNullOrEmpty(searchPhraseLower)
                || (
                    x.Name.ToLower().Contains(searchPhraseLower) && x.Name.ToLower().Contains(searchPhraseLower)
                    || x.Description.ToLower().Contains(searchPhraseLower)
                    || x.Category.ToLower().Contains(searchPhraseLower)
                )
            );

        int totalCount = await baseQuery.CountAsync();

        if (!string.IsNullOrEmpty(sortBy))
        {
            Dictionary<string, Expression<Func<Restaurant, object>>> columnSelector = new()
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category },
            };

            var selectedColumn = columnSelector[sortBy];
            baseQuery =
                (sortDirection == SortDirection.Ascending || sortDirection == null)
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
        }

        IEnumerable<Restaurant> restaurants = await baseQuery
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync();

        return (restaurants, totalCount);
    }

    public async Task<Restaurant?> GetByIdAsync(int id)
    {
        Restaurant? restaurant = await dbContext
            .Restaurants.Include(x => x.Dishes)
            .Include(x => x.Owner)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();
        return restaurant;
    }

    public async Task Delete(Restaurant entity)
    {
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public Task SaveChanges() => dbContext.SaveChangesAsync();
}
