using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public interface IRestaurantSeeder
{
    Task Seed();
}

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                IEnumerable<Restaurant> restaurants = RestaurantSeederService.GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }

            if (!dbContext.Roles.Any())
            {
                IEnumerable<IdentityRole> roles = RestaurantSeederService.GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
