namespace Restaurants.Application.Restaurants.Commands;

public interface IRestaurantCommand
{
    public string Name { get; set; }
    public string Description { get; set; }

    public string Category { get; set; }
    public string? ContactEmail { get; set; }
    public string? PostalCode { get; set; }
}
