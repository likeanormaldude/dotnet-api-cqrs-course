using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetRestaurantByIdQueryHandler> logger,
    IMapper mapper,
    IBlobStorageService blobStorageService
) : IRequestHandler<GetRestaurantByIdQuery, RestaurantDto>
{
    public async Task<RestaurantDto> Handle(GetRestaurantByIdQuery request, CancellationToken cancellationToken)
    {
        int id = request.Id;
        logger.LogInformation($"Getting restaurants by id {id}");
        Restaurant? restaurant =
            await restaurantsRepository.GetByIdAsync(id)
            ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
        RestaurantDto restaurantDto = mapper.Map<RestaurantDto>(restaurant ?? new object());
        restaurantDto.LogoSasUrl = blobStorageService.GetBlobSasUrl(restaurant?.LogoUrl);
        return restaurantDto;
    }
}
