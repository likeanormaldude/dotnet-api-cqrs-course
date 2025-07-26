using FluentAssertions;
using Microsoft.AspNetCore.Authorization;
using Moq;
using Restaurants.Application.Users;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Infrastructure.Authorization.Requirements.Tests;

public class CreatedMultipleRestaurantsRequirementHandlerTests
{
    [Fact()]
    public async Task HandleRequirementAsync_UserHasCreatedMultipleRestaurants_ShouldSucceed()
    {
        #region Arrange
        var userContextMock = new Mock<IUserContext>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        var currentUser = new CurrentUser("1", "test@test.com", [], null, null);
        userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = "2" },
        };

        restaurantsRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(
            restaurantsRepositoryMock.Object,
            userContextMock.Object
        );

        var context = new AuthorizationHandlerContext([requirement], null, null);

        #endregion

        #region Act
        await handler.HandleAsync(context);
        #endregion

        #region Assert
        context.HasSucceeded.Should().BeTrue();
        #endregion
    }

    [Fact]
    public async Task HandleRequirementAsync_UserHasNotCreatedMultipleRestaurants_ShouldFail()
    {
        #region Arrange
        var userContextMock = new Mock<IUserContext>();
        var restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        var currentUser = new CurrentUser("2", "test@test.com", [], null, null);
        userContextMock.Setup(x => x.GetCurrentUser()).Returns(currentUser);

        var restaurants = new List<Restaurant>
        {
            new Restaurant { OwnerId = "1" },
            new Restaurant { OwnerId = "1" },
            new Restaurant { OwnerId = currentUser.Id },
        };

        restaurantsRepositoryMock.Setup(x => x.GetAllAsync()).ReturnsAsync(restaurants);

        var requirement = new CreatedMultipleRestaurantsRequirement(2);
        var handler = new CreatedMultipleRestaurantsRequirementHandler(
            restaurantsRepositoryMock.Object,
            userContextMock.Object
        );

        var context = new AuthorizationHandlerContext([requirement], null, null);

        #endregion

        #region Act
        await handler.HandleAsync(context);
        #endregion

        #region Assert
        context.HasFailed.Should().BeTrue();
        #endregion
    }
}
