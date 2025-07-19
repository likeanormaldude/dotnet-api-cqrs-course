using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantDto
{
    public int Id { get; set; } = default;
    public string Name { get; set; } = default!;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool HasDelivery { get; set; } = default;

    //public string? ContactEmail { get; set; }
    //public string? ContactNumber { get; set; }
    public string City { get; set; } = default!;
    public string? Street { get; set; }
    public string? PostalCode { get; set; }
    public List<DishDto> Dishes { get; set; } = [];
    public string Owner { get; set; } = default!;
}
