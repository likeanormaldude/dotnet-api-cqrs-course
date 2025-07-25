using System.Net;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Restaurants.API.Controllers.Tests;

public class RestaurantsControllerTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    public const string apiUrl = "/api/restaurants";

    [Fact()]
    public async Task GetAll_ForValidRequest_Returns200Ok()
    {
        #region Arrange
        var client = factory.CreateClient();
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
        var client = factory.CreateClient();
        //string queryString = "?pageNumber=1&pageSize=10";
        #endregion

        #region Act
        var result = await client.GetAsync($"{apiUrl}");
        #endregion

        #region Assert
        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        #endregion
    }
}
