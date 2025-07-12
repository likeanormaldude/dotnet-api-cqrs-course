using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Extensions;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator, IValidator<CreateDishCommand> createValidator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;

        ValidationResult validationResults = await createValidator.ValidateAsync(command);

        // TODO - Create middleware to handle validation errors globally
        if (!validationResults.IsValid)
            return BadRequest(validationResults.Errors.ToGroupedValidationErrors().MapOnlyErrorMessages());

        await mediator.Send(command);
        return Created();
    }
}
