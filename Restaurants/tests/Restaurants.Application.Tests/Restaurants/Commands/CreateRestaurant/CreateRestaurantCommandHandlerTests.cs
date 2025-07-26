using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant.Tests;

public class CreateRestaurantCommandHandlerTests
{
    [Fact()]
    public async Task Handle_ForValidCommand_ReturnsCreatedRestaurantId()
    {
        #region Arrange
        var loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        var mapperMock = new Mock<IMapper>();

        var userContextMock = new Mock<IUserContext>();
        var currentUser = new CurrentUser("owner-id", "test@test.com", [], null, null);
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);

        Restaurant restaurant = new()
        {
            Id = 1,
            Name = "Test Restaurant",
            Description = "A test restaurant for unit testing",
            OwnerId = "owner-id",
        };

        CreateRestaurantCommand command = new() { Name = restaurant.Name, Description = restaurant.Description };

        mapperMock.Setup(m => m.Map<Restaurant>(command)).Returns(restaurant);

        var restaurantRepositoryMock = new Mock<IRestaurantsRepository>();

        restaurantRepositoryMock.Setup(repo => repo.Create(restaurant)).ReturnsAsync(1);

        var commandHandler = new CreateRestaurantCommandHandler(
            loggerMock.Object,
            mapperMock.Object,
            restaurantRepositoryMock.Object,
            userContextMock.Object
        );
        #endregion

        #region Act
        int result = await commandHandler.Handle(command, CancellationToken.None);
        #endregion

        #region Assert
        result.Should().Be(1);
        restaurant.OwnerId.Should().Be(currentUser.Id);
        restaurantRepositoryMock.Verify(repo => repo.Create(restaurant), Times.Once);
        #endregion
    }
}
