using Bogus;
using EFCore.BulkExtensions;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.SeedScripts;

public class Insert5kRestaurants : IDataScript<RestaurantsDbContext>
{
    public string Name => nameof(Insert5kRestaurants);

    public async Task RunAsync(RestaurantsDbContext context, IServiceProvider services)
    {
        //int initialId = 4500;
        string ownerId = context.Users.FirstOrDefault()?.Id ?? "";
        User owner = context.Users.FirstOrDefault(u => u.Id == ownerId)!;

        string[] suffixes = { "Bistro", "Bar & Grill", "Diner", "Kitchen", "Eatery", "Tavern", "Quitanda" };
        string[] dishCategories = { "Appetizer", "Main Course", "Dessert", "Beverage" };

        var restaurantFaker = new Faker<Restaurant>()
            .StrictMode(false)
            //.RuleFor(f => f.Id, s => initialId++)
            .RuleFor(f => f.Name, s => $"{s.Company.CompanyName()} {s.PickRandom(suffixes)}")
            .RuleFor(f => f.Description, s => s.Lorem.Sentence())
            .RuleFor(f => f.Category, s => s.Commerce.Categories(1).FirstOrDefault() ?? "General")
            .RuleFor(f => f.HasDelivery, s => s.Random.Bool(0.5f))
            .RuleFor(f => f.ContactEmail, s => s.Internet.Email())
            .RuleFor(f => f.ContactNumber, s => s.Phone.PhoneNumber())
            .RuleFor(
                f => f.Address,
                s => new Address
                {
                    Street = s.Address.StreetName(),
                    City = s.Address.City(),
                    PostalCode = s.Address.ZipCode(),
                }
            )
            .RuleFor(
                f => f.Dishes,
                s => new List<Dish>
                {
                    new Dish
                    {
                        Name = s.Name.FirstName(),
                        Description = s.Lorem.Sentence(),
                        Price = s.Finance.Amount(5, 100),
                        KiloCalories = s.Random.Int(100, 1000),
                    },
                }
            )
            .RuleFor(f => f.Owner, s => owner)
            .RuleFor(f => f.OwnerId, s => owner.Id);

        List<Restaurant> restaurants = restaurantFaker.Generate(5000);

        context.Restaurants.AddRange(restaurants);

        await context.BulkInsertAsync(restaurants);

        // Clear the context tracking for all entities
        context.ChangeTracker.Clear();
    }
}
