using System.ComponentModel.DataAnnotations;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Insert a valid category")]
    public string Category { get; set; } = default!;
    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "Please provide a valid phone number")]
    public string? ContactNumber { get; set; }
    public string City { get; set; } = default!;
    public string? Street { get; set; }

    [RegularExpression(
        @"^((\S{3})(\s{1})?(\S{3}))$",
        ErrorMessage = "Please provide a valid postal code in the format \"XXX XXX\""
    )]
    public string? PostalCode { get; set; }
    public List<DishDto> Dishes { get; set; } = [];
}
