using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Extensions;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantById;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/restaurants")]
public class RestaurantsController(
    IMediator mediator,
    IValidator<CreateRestaurantCommand> createValidator,
    IValidator<UpdateRestaurantCommand> updateValidator
) : ControllerBase
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
        ValidationResult validationResults = await createValidator.ValidateAsync(command);

        if (!validationResults.IsValid)
            return BadRequest(validationResults.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());

        int id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, new { id });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        DeleteRestaurantCommand command = new(id);
        bool isDeleted = await mediator.Send(command);

        if (isDeleted)
        {
            return NoContent();
        }

        return NotFound();
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantCommand command)
    {
        command.Id = id;
        ValidationResult validationResults = await updateValidator.ValidateAsync(command);

        if (!validationResults.IsValid)
            return BadRequest(validationResults.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());

        bool isUpdated = await mediator.Send(command);

        if (isUpdated)
        {
            return NoContent();
        }

        return NotFound();
    }
}
