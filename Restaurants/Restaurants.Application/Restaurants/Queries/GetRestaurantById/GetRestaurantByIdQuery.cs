using MediatR;
using Restaurants.Application.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery: IRequest<RestaurantDto?>
{
    public int Id { get; set; }

    public GetRestaurantByIdQuery(int id)
    {
        Id = id;
    }
}
