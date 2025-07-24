using FluentAssertions;
using Mapster;
using Restaurants.Application.Common;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Xunit;

namespace Restaurants.Application.Mappings.Tests;

public class RestaurantMappingsTests
{
    private TypeAdapterConfig configuration;

    public RestaurantMappingsTests()
    {
        configuration = MapsterApplicationHelper.RegisterAll();
    }

    [Fact()]
    public void CreateMap_ForRestaurantToRestaurantDto_MapsCorrectly()
    {
        #region Arrange
        Restaurant restaurant = new()
        {
            Id = 1,
            Name = "Test Restaurant",
            Address = new Address
            {
                City = "Test City",
                Street = "Test Street",
                PostalCode = "12345",
            },
            Dishes = new List<Dish>
            {
                new Dish
                {
                    Id = 1,
                    Name = "Test Dish",
                    Price = 10.0m,
                },
            },
            Owner = new User { UserName = "testowner" },
        };
        #endregion

        #region Act
        RestaurantDto restaurantDto = restaurant.Adapt<RestaurantDto>(configuration);
        #endregion

        #region Assert
        restaurantDto.Should().NotBeNull();
        restaurantDto.Id.Should().Be(restaurant.Id);
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        restaurantDto.Category.Should().Be(restaurant.Category);
        restaurantDto.HasDelivery.Should().Be(restaurant.HasDelivery);
        restaurantDto.City.Should().Be(restaurant.Address.City);
        restaurantDto.Street.Should().Be(restaurant.Address.Street);
        restaurantDto.PostalCode.Should().Be(restaurant.Address.PostalCode);
        #endregion
    }

    [Fact()]
    public void CreateMap_ForCreateRestaurantCommand_MapsCorrectly()
    {
        #region Arrange
        CreateRestaurantCommand command = new()
        {
            Name = "Test Restaurant",
            Description = "A test restaurant",
            Category = "Italian",
            HasDelivery = true,
            ContactEmail = "test@test.com",
            ContactNumber = "1234567890",
            City = "Test City",
        };
        #endregion

        #region Act
        Restaurant restaurant = command.Adapt<Restaurant>(configuration);
        #endregion

        #region Assert
        restaurant.Should().NotBeNull();
        restaurant.Name.Should().Be(command.Name);
        restaurant.Description.Should().Be(command.Description);
        restaurant.Category.Should().Be(command.Category);
        restaurant.HasDelivery.Should().Be(command.HasDelivery);
        restaurant.ContactEmail.Should().Be(command.ContactEmail);
        restaurant.ContactNumber.Should().Be(command.ContactNumber);
        restaurant.Address!.City.Should().Be(command.City);
        #endregion
    }
}
