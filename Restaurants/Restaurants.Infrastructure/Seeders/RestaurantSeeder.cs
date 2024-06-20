using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Seeders
{
    internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
    {
        public async Task Seed()
        {
            if (await dbContext.Database.CanConnectAsync())
            {
                if (!dbContext.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    dbContext.Restaurants.AddRange(restaurants);
                    await dbContext.SaveChangesAsync();
                }
            }
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            List<Restaurant> restaurants = new()
            {
                new Restaurant
                {
                    Name = "Pizzeria Bella",
                    Description = "Cozy pizzeria with a variety of traditional and gourmet pizzas.",
                    Category = "Italian",
                    HasDelivery = true,
                    ContactEmail = "info@pizzeriabella.com",
                    ContactNumber = "+1234567890",
                    Address = new Address
                    {
                        City = "New York",
                        Street = "123 Main Street",
                        PostalCode = "10001"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish {Name = "Margherita Pizza", Description = "Classic pizza with tomato, mozzarella, and basil.", Price = 10.99m },
                        new Dish {Name = "Pepperoni Pizza", Description = "Pizza topped with pepperoni slices and mozzarella cheese.", Price = 12.99m },
                        new Dish {Name = "Vegetarian Pizza", Description = "Pizza loaded with fresh vegetables and cheese.", Price = 11.99m }
                    }
                },
                new Restaurant
                {
                    Name = "Sushi Delight",
                    Description = "Authentic Japanese sushi restaurant offering a variety of sushi rolls and sashimi.",
                    Category = "Japanese",
                    HasDelivery = false,
                    ContactEmail = "info@sushidelight.com",
                    ContactNumber = "+1987654321",
                    Address = new Address
                    {
                        City = "Los Angeles",
                        Street = "456 Oak Avenue",
                        PostalCode = "90002"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish {Name = "California Roll", Description = "Sushi roll with crab, avocado, and cucumber.", Price = 8.50m },
                        new Dish {Name = "Sashimi Platter", Description = "Assortment of fresh raw fish slices.", Price = 15.99m },
                        new Dish {Name = "Dragon Roll", Description = "Sushi roll with eel, avocado, and cucumber topped with avocado slices.", Price = 12.99m }
                    }
                },
                new Restaurant
                {
                    Name = "Taste of India",
                    Description = "Indian restaurant offering a rich variety of curries, tandoori dishes, and biryanis.",
                    Category = "Indian",
                    HasDelivery = true,
                    ContactEmail = "info@tasteofindia.com",
                    ContactNumber = "+1555123456",
                    Address = new Address
                    {
                        City = "Chicago",
                        Street = "789 Elm Street",
                        PostalCode = "60601"
                    },
                    Dishes = new List<Dish>
                    {
                        new Dish {Name = "Chicken Tikka Masala", Description = "Grilled chicken in a creamy tomato sauce.", Price = 14.99m },
                        new Dish {Name = "Vegetable Biryani", Description = "Flavorful rice dish with mixed vegetables and aromatic spices.", Price = 12.50m },
                        new Dish {Name = "Paneer Butter Masala", Description = "Paneer cubes cooked in a buttery tomato sauce.", Price = 13.99m }
                    }
                }
            };

            return restaurants;
        }
    }
}
