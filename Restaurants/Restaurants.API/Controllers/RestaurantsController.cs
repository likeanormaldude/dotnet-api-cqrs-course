using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Extensions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(IMediator mediator, IValidator<CreateRestaurantCommand> createValidator)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        GetAllRestaurantsQuery query = new();
        IEnumerable<RestaurantDto> restaurants = await mediator.Send(query);
        return Ok(restaurants);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        GetRestaurantByIdQuery query = new(id);
        RestaurantDto? restaurant = await mediator.Send(query);

        if (restaurant == null)
        {
            return NotFound($"Restaurant with id {id} could not be found.");
        }

        return Ok(restaurant);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
    {
        var validationResults = await createValidator.ValidateAsync(command);

        if (!validationResults.IsValid)
            return BadRequest(validationResults.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());

        int id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }
}
