using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Restaurant> restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Restaurant? restaurants = await restaurantsService.GetRestaurantById(id);

        if (restaurants == null)
        {
            return NotFound($"Restaurant with id {id} could not be found.");
        }

        return Ok(restaurants);
    }
}
