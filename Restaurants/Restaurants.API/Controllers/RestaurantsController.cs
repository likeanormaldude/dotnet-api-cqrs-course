using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IRestaurantsService restaurantsService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<RestaurantDto> restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        RestaurantDto? restaurants = await restaurantsService.GetRestaurantById(id);

        if (restaurants == null)
        {
            return NotFound($"Restaurant with id {id} could not be found.");
        }

        return Ok(restaurants);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRestaurantDto createRestaurant)
    {
        int id = await restaurantsService.Create(createRestaurant);

        return CreatedAtAction(nameof(GetById), new { id }, null);
    }
}
