using MediatR;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant;

public class CreateRestaurantCommand : IRequest<int>, IRestaurantCommand
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public bool HasDelivery { get; set; }

    public string? ContactEmail { get; set; } = default!;

    public string? ContactNumber { get; set; } = default!;

    public string? City { get; set; }
    public string? Street { get; set; }

    public string? PostalCode { get; set; }

    public List<Dish> Dishes { get; set; } = new();
}
