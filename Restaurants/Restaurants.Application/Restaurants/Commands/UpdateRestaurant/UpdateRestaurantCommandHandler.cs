using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository
): IRequestHandler<UpdateRestaurantCommand, IEnumerable<Restaurant>>
{
    public async Task<IEnumerable<Restaurant>?> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating restaurant with id {RestaurantId} with {@UpdatedRestaurant}", request.Id, request);

        var found = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);

        if (found == null)
            return null;

        var updatedRestaurant = mapper.Map(request, found);
        await restaurantsRepository.SaveChanges();

        return await restaurantsRepository.GetAllRestaurantsAsync();
    }
}
