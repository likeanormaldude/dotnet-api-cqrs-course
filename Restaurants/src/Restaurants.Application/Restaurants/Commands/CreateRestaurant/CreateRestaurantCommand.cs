using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommand : IRequest<int>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; } = default;

    public string? ContactEmail { get; set; }

    public string? ContactNumber { get; set; }
    public string City { get; set; } = string.Empty;
    public string? Street { get; set; }

    public string? PostalCode { get; set; }
    public List<DishDto> Dishes { get; set; } = [];
}
