using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using System.Text.Json;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommandHandler(
    ILogger logger,
    IMapper mapper,
    IRestaurantsRepository restaurantsRepository
): IRequestHandler<UpdateRestaurantCommand, IEnumerable<Restaurant>>
{
    public async Task<IEnumerable<Restaurant>?> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var serialized = JsonSerializer.Serialize(request);

        logger.LogInformation("Updating restaurant with id {RestaurantId} with {UpdatedRestaurant}", request.Id, serialized);

        var found = await restaurantsRepository.GetRestaurantByIdAsync(request.Id);

        if (found == null)
            return null;

        var updatedRestaurant = mapper.Map(request, found);
        await restaurantsRepository.SaveChanges();

        return await restaurantsRepository.GetAllRestaurantsAsync();
    }
}
