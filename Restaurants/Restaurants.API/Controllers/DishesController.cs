using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;
using Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaurant;
using Restaurants.Application.Dishes.Commands.DeleteDishByIdForRestaurant;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant;
using Restaurants.Application.Dishes.Queries.GetDishesForRestaurant;
using Restaurants.Infrastructure.Authorization;

namespace Restaurants.API.Controllers;

[Route("api/restaurants/{restaurantId}/dishes")]
[Authorize]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, [FromBody] CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        int dishId = await mediator.Send(command);
        object routeValues = new { restaurantId, dishId };
        return CreatedAtAction(nameof(GetByIdForRestaurant), routeValues, dishId);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DishDto>>> GetAllForRestaurant([FromRoute] int restaurantId)
    {
        IEnumerable<DishDto> dishes = await mediator.Send(new GetDishesForRestaurantQuery(restaurantId));

        return Ok(dishes);
    }

    [HttpGet("{dishId}")]
    [Authorize(Policy = PolicyNames.AtLeast20)]
    public async Task<ActionResult<DishDto>> GetByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        DishDto dish = await mediator.Send(new GetDishByIdForRestaurantQuery(restaurantId, dishId));
        return Ok(dish);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAllDishesForRestaurant([FromRoute] int restaurantId)
    {
        var command = new DeleteAllDishesForRestaurantCommand(restaurantId);
        bool isDeleted = await mediator.Send(command);

        if (isDeleted)
        {
            return NoContent();
        }

        return BadRequest("Dish not found or could not be deleted.");
    }

    [HttpDelete("{dishId}")]
    public async Task<IActionResult> DeleteDishByIdForRestaurant([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var command = new DeleteDishByIdForRestaurantCommand(restaurantId, dishId);
        bool isDeleted = await mediator.Send(command);

        if (isDeleted)
        {
            return NoContent();
        }

        return BadRequest("Dish not found or could not be deleted.");
    }
}
