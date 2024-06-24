using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurants;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

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
}