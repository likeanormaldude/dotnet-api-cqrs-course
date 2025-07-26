using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteDishByIdForRestaurant;

internal class DeleteDishByIdForRestaurantCommandHandler(
    ILogger<DeleteDishByIdForRestaurantCommandHandler> logger,
    IRestaurantsRepository restaurantsRepository,
    IDishesRepository dishesRepository,
    IRestaurantAuthorizationService restaurantAuthorizationService
) : IRequestHandler<DeleteDishByIdForRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteDishByIdForRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Deleting Dish (id: {DishId}) for restaurant ({RestaurantId})",
            request.DishId,
            request.RestaurantId
        );

        var restaurant = await restaurantsRepository.GetByIdAsync(request.RestaurantId);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId.ToString());
        }

        if (!restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
        {
            throw new ForbidException();
        }

        int index = restaurant.Dishes.FindIndex(x => x.Id == request.DishId);

        // If not found
        if (index < 0)
        {
            throw new NotFoundException(nameof(Dish), request.DishId.ToString());
        }

        await dishesRepository.Delete(restaurant.Dishes[index]);
        return true;
    }
}
