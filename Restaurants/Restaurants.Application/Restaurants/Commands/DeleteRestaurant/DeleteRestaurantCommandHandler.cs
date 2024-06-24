using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

internal class DeleteRestaurantCommandHandler(
    ILogger logger,
    IRestaurantsRepository restaurantsRepository
): IRequestHandler<DeleteRestaurantCommand, bool>
{
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation($"Delete restaurant with id {request.Id}");

        var restaurant = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);

        if (restaurant == null)
            return false;

        await restaurantsRepository.DeleteRestaurant(restaurant);

        return true;
    }
}
