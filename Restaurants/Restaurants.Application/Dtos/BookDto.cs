using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int NumberOfPages { get; set; }
}
