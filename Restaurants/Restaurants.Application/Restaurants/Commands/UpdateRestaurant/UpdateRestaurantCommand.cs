using MediatR;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant;

public class UpdateRestaurantCommand(int id) : IRequest<IEnumerable<Restaurant>>, IRestaurantCommand
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; }
    public string? ContactEmail { get ; set; } = string.Empty;
    public string? PostalCode { get; set; } = string.Empty;
}
