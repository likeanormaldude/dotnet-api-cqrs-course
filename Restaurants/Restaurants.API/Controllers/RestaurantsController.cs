using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dtos;
using Restaurants.Application.Restaurants;

namespace Restaurants.API.Controllers;


[ApiController]
[Route("api/restaurants")]
public class RestaurantsController
(
    IRestaurantsService restaurantsService
): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult>  GetAll()
    {
        var restaurants = await restaurantsService.GetAllRestaurants();
        return Ok(restaurants);
    }

    // GET - .https://localhost:7255/api/restaurants/12
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurant([FromRoute] int id)
    {
        var restaurant = await restaurantsService.GetById(id);

        if(restaurant == null)
        {
            return NotFound();
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto createRestaurantDto)
    {
        int id = await restaurantsService.Create(createRestaurantDto);

        /*
         This line returns the property "Location" in the headers point to the location
         of the method "GetRestaurant", for the user to see the newly create record
         */
        return CreatedAtAction(nameof(GetRestaurant), new { id }, "Restaurant created successfully" );
    }
}