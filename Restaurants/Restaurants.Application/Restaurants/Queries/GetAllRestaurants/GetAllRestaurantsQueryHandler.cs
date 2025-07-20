using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants;

public class GetAllRestaurantsQueryHandler(
    IRestaurantsRepository restaurantsRepository,
    ILogger<GetAllRestaurantsQueryHandler> logger,
    IMapper mapper
) : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
{
    public async Task<IEnumerable<RestaurantDto>> Handle(
        GetAllRestaurantsQuery request,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation("Getting all restaurants");
        IEnumerable<Restaurant> restaurants = await restaurantsRepository.GetAllMatchingAsync(request.SearchPhrase);
        IEnumerable<RestaurantDto> restaurantDtos = mapper.Map<IEnumerable<Restaurant>, IEnumerable<RestaurantDto>>(
            restaurants
        );

        return restaurantDtos;
    }
}
