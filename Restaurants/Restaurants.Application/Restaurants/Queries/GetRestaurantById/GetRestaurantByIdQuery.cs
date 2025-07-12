using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery : IRequest<RestaurantDto>
{
    public int Id { get; set; }

    public GetRestaurantByIdQuery(int id)
    {
        Id = id;
    }
}
