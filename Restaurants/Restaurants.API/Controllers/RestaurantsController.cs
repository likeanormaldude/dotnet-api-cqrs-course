using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Extensions;
using Restaurants.Application.Restaurants;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(
    IRestaurantsService restaurantsService,
    IValidator<CreateRestaurantDto> createValidator
) : ControllerBase
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
        RestaurantDto? restaurant = await restaurantsService.GetRestaurantById(id);

        if (restaurant == null)
        {
            return NotFound($"Restaurant with id {id} could not be found.");
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRestaurantDto createRestaurant)
    {
        var validationResults = await createValidator.ValidateAsync(createRestaurant);

        if (!validationResults.IsValid)
            return BadRequest(validationResults.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());

        int id = await restaurantsService.Create(createRestaurant);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
