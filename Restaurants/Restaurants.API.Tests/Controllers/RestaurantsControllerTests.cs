using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Restaurants.API.Tests;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    public const string apiUrl = "/api/restaurants";
    private WebApplicationFactory<Program> _factory;
    private Mock<IRestaurantsRepository> _restaurantsRepositoryMock = new();

    public RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();

                // For it to replace the existing IRestaurantsRepository registration in the ServiceCollectionExtensions
                services.Replace(
                    ServiceDescriptor.Scoped(typeof(IRestaurantsRepository), _ => _restaurantsRepositoryMock.Object)
                );
            });
        });
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        #region Arrange
        var client = _factory.CreateClient();
        string queryString = "?pageNumber=1&pageSize=10";
        #endregion

        #region Act
        var result = await client.GetAsync($"{apiUrl}{queryString}");
        #endregion

        #region Assert
        result.StatusCode.Should().Be(HttpStatusCode.OK);
        #endregion
    }

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns400BadRequest()
    {
        #region Arrange
        var client = _factory.CreateClient();
        //string queryString = "?pageNumber=1&pageSize=10";
        #endregion

        #region Act
        var response = await client.GetAsync($"{apiUrl}");
        #endregion

        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        #endregion
    }

    [Fact()]
    public async Task GetById_ForNonExistentId_ShouldReturn404NotFound()
    {
        #region Arrange
        int idMock = 999;
        var client = _factory.CreateClient();
        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(idMock)).ReturnsAsync((Restaurant?)null);
        #endregion

        #region Act
        var response = await client.GetAsync($"{apiUrl}/{idMock}");
        #endregion

        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        #endregion
    }

    [Fact()]
    public async Task GetById_ForExistentId_ShouldReturn200Ok()
    {
        #region Arrange
        int idMock = 99;

        Restaurant restaurant = new Restaurant
        {
            Id = idMock,
            Name = "Test Restaurant",
            Description = "A test restaurant for unit testing",
            Category = "Test Category",
            HasDelivery = true,
            ContactEmail = "test@test.com",
        };

        var client = _factory.CreateClient();
        _restaurantsRepositoryMock.Setup(repo => repo.GetByIdAsync(idMock)).ReturnsAsync(restaurant);
        #endregion

        #region Act
        var response = await client.GetAsync($"{apiUrl}/{idMock}");
        var restaurantDto = await response.Content.ReadFromJsonAsync<RestaurantDto>();
        #endregion

        #region Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        restaurantDto.Should().NotBeNull();
        restaurantDto.Name.Should().Be(restaurant.Name);
        restaurantDto.Description.Should().Be(restaurant.Description);
        #endregion
    }
}
