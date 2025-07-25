using System.Security.Claims;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Application.Users;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant.Tests;

public class UpdateRestaurantCommandHandlerTests
{
    private Restaurant restaurant;
    private Mock<IRestaurantAuthorizationService> authorizationMock;
    private Mock<IRestaurantsRepository> restaurantsRepository;
    private Mock<ILogger<UpdateRestaurantCommandHandler>> loggerMock;
    private Mock<IMapper> mapperMock;
    private Mock<IUserContext> userContextMock;
    private const int restaurantIdMock = 1;
    private const string onwerIdMock = "owner-id";

    public UpdateRestaurantCommandHandlerTests()
    {
        restaurantsRepository = new Mock<IRestaurantsRepository>();
        authorizationMock = new Mock<IRestaurantAuthorizationService>();
        loggerMock = new Mock<ILogger<UpdateRestaurantCommandHandler>>();
        mapperMock = new Mock<IMapper>();
        userContextMock = new Mock<IUserContext>();

        restaurant = new Restaurant()
        {
            Id = restaurantIdMock,
            Name = "Test Restaurant",
            Description = "A test restaurant for unit testing",
            OwnerId = onwerIdMock,
            HasDelivery = true,
        };

        authorizationMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update)).Returns(true);
        restaurantsRepository.Setup(repo => repo.GetByIdAsync(restaurant.Id)).ReturnsAsync(restaurant);

        var currentUser = new CurrentUser(onwerIdMock, "owner@test.com", [UserRoles.Owner], null, null);
        userContextMock.Setup(uc => uc.GetCurrentUser()).Returns(currentUser);
    }

    [Fact()]
    public async Task Handle_ForValidCommand_CompareUpdatedProps()
    {
        #region Arrange
        var loggerForQueryMock = new Mock<ILogger<GetRestaurantByIdQueryHandler>>();

        UpdateRestaurantCommand command = new(restaurantIdMock)
        {
            Name = "Updated Restaurant",
            Description = "Updated description",
            Category = "Updated Category",
        };

        mapperMock.Setup(m => m.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        var restaurantDto = new RestaurantDto
        {
            Id = restaurant.Id,
            Name = command.Name,
            Description = command.Description,
            Category = command.Category,
        };

        mapperMock.Setup(m => m.Map<RestaurantDto>(restaurant)).Returns(restaurantDto);

        UpdateRestaurantCommandHandler commandHandler = new(
            loggerMock.Object,
            restaurantsRepository.Object,
            mapperMock.Object,
            authorizationMock.Object
        );

        #endregion

        #region Act
        await commandHandler.Handle(command, CancellationToken.None);

        var query = new GetRestaurantByIdQuery(restaurantIdMock);
        var queryHandler = new GetRestaurantByIdQueryHandler(
            restaurantsRepository.Object,
            loggerForQueryMock.Object,
            mapperMock.Object
        );

        RestaurantDto restaurantAfterUpdate = await queryHandler.Handle(query, CancellationToken.None);
        #endregion

        #region Assert
        mapperMock.Verify(x => x.Map(command, restaurant), Times.Once);
        restaurantsRepository.Verify(x => x.SaveChanges(), Times.Once);
        restaurantAfterUpdate.Should().NotBeNull();
        restaurantAfterUpdate.Id.Should().Be(restaurant.Id);
        restaurantAfterUpdate.Name.Should().Be(command.Name);
        restaurantAfterUpdate.Description.Should().Be(command.Description);
        restaurantAfterUpdate.Category.Should().Be(command.Category);
        #endregion
    }

    [Fact]
    public async Task Handle_ForInvalidCommand_ShouldThrowNotFoundException()
    {
        #region Arrange
        int dummyRestaurantId = 999; // A restaurant ID that does not exist
        // Arrange
        UpdateRestaurantCommand command = new(dummyRestaurantId)
        {
            Name = "Updated Restaurant",
            Description = "Updated description",
            Category = "Updated Category",
        };
        mapperMock.Setup(m => m.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);
        restaurantsRepository.Setup(repo => repo.GetByIdAsync(command.Id)).ReturnsAsync((Restaurant?)null);

        var restaurantDto = new RestaurantDto
        {
            Id = restaurant.Id,
            Name = command.Name,
            Description = command.Description,
            Category = command.Category,
        };

        mapperMock.Setup(m => m.Map<RestaurantDto>(restaurant)).Returns(restaurantDto);

        var commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            restaurantsRepository.Object,
            mapperMock.Object,
            authorizationMock.Object
        );
        #endregion

        #region Act
        Func<Task> result = () => commandHandler.Handle(command, CancellationToken.None);
        #endregion

        #region Assert
        await result.Should().ThrowAsync<NotFoundException>();
        #endregion
    }

    [Fact]
    public async Task Handle_ForUnauthorizedUser_ShouldThrowForbidException()
    {
        #region Arrange
        UpdateRestaurantCommand command = new(restaurantIdMock)
        {
            Name = "Updated Restaurant",
            Description = "Updated description",
            Category = "Updated Category",
        };

        mapperMock.Setup(m => m.Map<UpdateRestaurantCommand, Restaurant>(command)).Returns(restaurant);

        // To have HttpContext not null, but User null:
        var httpContextMock = new Mock<HttpContext>();
        httpContextMock.Setup(c => c.User).Returns((ClaimsPrincipal?)null);

        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        authorizationMock.Setup(auth => auth.Authorize(restaurant, ResourceOperation.Update)).Returns(false);

        var commandHandler = new UpdateRestaurantCommandHandler(
            loggerMock.Object,
            restaurantsRepository.Object,
            mapperMock.Object,
            authorizationMock.Object
        );
        #endregion

        #region Act
        Func<Task> result = () => commandHandler.Handle(command, CancellationToken.None);
        #endregion

        #region Assert
        await result.Should().ThrowAsync<ForbidException>();
        #endregion
    }
}
