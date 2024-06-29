using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;
using Restaurants.Domain.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController
(
    IMediator mediator
): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        var query = new GetAllRestaurantsQuery();
        var restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    // GET - .https://localhost:7255/api/restaurants/12
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant([FromRoute] int id)
    {
        var query = new GetRestaurantByIdQuery(id);
        var restaurant = await mediator.Send(query);

        if(restaurant == null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
    {
        int id = await mediator.Send(command);

        /*
         This line returns the property "Location" in the headers point to the location
         of the method "GetRestaurant", for the user to see the newly create record
         */
        return CreatedAtAction(nameof(GetRestaurant), new { id }, "Restaurant created successfully" );
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        var command = new DeleteRestaurantCommand(id);
        var isDeleted = await mediator.Send(command);

        if (isDeleted)
            return NoContent();

        return NotFound();
    }


    // PATCH - .https://localhost:7255/api/restaurants/12
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand newRestaurant)
    {
        newRestaurant.Id = id;
        var result = await mediator.Send(newRestaurant);

        if (!result.IsEmptyList())
            return Ok(result);

        return NotFound();
    }
}