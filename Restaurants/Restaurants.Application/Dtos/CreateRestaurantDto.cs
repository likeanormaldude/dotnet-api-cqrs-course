using Restaurants.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Dtos;

public class CreateRestaurantDto
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Insert a valid category")]
    public string Category { get; set; } = string.Empty;

    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public string? ContactEmail { get; set; } = default!;

    [Phone(ErrorMessage = "Please provide a valid phone number")]
    public string? ContactNumber { get; set; } = default!;

    public string? City { get; set; }
    public string? Street { get; set; }


    [RegularExpression(@"^(\S{3})(\s)?(\S{3})$", ErrorMessage = "Please provide a valid post code (XXX XXX).")]
    public string? PostalCode { get; set; }
    
    public List<Dish> Dishes { get; set; } = new();
}
