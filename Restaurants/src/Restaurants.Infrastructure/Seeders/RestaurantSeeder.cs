using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public interface IRestaurantSeeder
{
    Task Seed();
}

internal class RestaurantSeeder(RestaurantsDbContext dbContext, UserManager<User> userManager) : IRestaurantSeeder
{
    public async Task Seed()
    {
        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (!await dbContext.Database.CanConnectAsync())
            return;

        await SeedDefaultOwnerAsync();
        await SeedRestaurantsAsync();
        await SeedRolesAsync();
    }

    private async Task SeedDefaultOwnerAsync()
    {
        var existingOwner = await userManager.FindByNameAsync("OWNER");
        if (existingOwner != null)
            return;

        var owner = new User
        {
            UserName = "OWNER",
            NormalizedUserName = "OWNER",
            Email = "test@test.com",
            NormalizedEmail = "TEST@TEST.COM",
        };

        // Set a strong dev/test password
        string password = "VCxhUd5V#wNcMU8*wzYg";

        var result = await userManager.CreateAsync(owner, password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create OWNER user: {errors}");
        }
    }

    private async Task SeedRestaurantsAsync()
    {
        bool hasRestaurants = await dbContext.Restaurants.AnyAsync();
        if (hasRestaurants)
            return;

        var owner = await dbContext.Users.FirstAsync(u => u.UserName == "OWNER");

        var restaurants = RestaurantSeederService
            .GetRestaurants()
            .Select(r =>
            {
                r.OwnerId = owner.Id;
                return r;
            })
            .ToList();

        await dbContext.Restaurants.AddRangeAsync(restaurants);
        await dbContext.SaveChangesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (await dbContext.Roles.AnyAsync())
            return;

        var roles = RestaurantSeederService.GetRoles().ToList();
        dbContext.Roles.AddRange(roles);
        await dbContext.SaveChangesAsync();
    }
}
