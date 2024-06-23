using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                // Restaurants
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();
                }

                // Books
                if (!dbContext.Books.Any())
                {
                    var books = GetBooks();

                    dbContext.Books.AddRange(books);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new()
            {
                new()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description =
                        "KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery = true,
                    Dishes =
                    [
                        new ()
                        {
                            Name = "Nashville Hot Chicken",
                            Description = "Nashville Hot Chicken (10 pcs.)",
                            Price = 10.30M,
                        },
                        new ()
                        {
                            Name = "Chicken Nuggets",
                            Description = "Chicken Nuggets (5 pcs.)",
                            Price = 5.30M,
                        },
                    ],
                    Address = new ()
                    {
                        City = "London",
                        Street = "Cork St 5",
                        PostalCode = "WC2N 5DU"
                    },
                },
                    new ()
                {
                    Name = "McDonald",
                    Category = "Fast Food",
                    Description =
                        "McDonald's Corporation (McDonald's), incorporated on December 21, 1964, operates and franchises McDonald's restaurants.",
                    ContactEmail = "contact@mcdonald.com",
                    HasDelivery = true,
                    Address = new Address()
                    {
                        City = "London",
                        Street = "Boots 193",
                        PostalCode = "W1F 8SR"
                    }
                }
            };

            return restaurants;
        }

        private IEnumerable<Book> GetBooks()
        {
            List<Book> books = new()
            {
               new Book()
               {
                   Title = "O rapaz que não era de Liverpool",
                   Description = "Os Beatles eram 4. Somente 4.",
                   NumberOfPages = 190
               },
               new Book()
               {
                   Title = "The Book Thief",
                   Description = "Primeiro as cores, depois as pessoas. É assim que eu vejo a vida. Uma pequena nota de sua narradora. Você vai morrer.",
                   NumberOfPages = 592
               },
            };

            return books;
        }
    }
}
