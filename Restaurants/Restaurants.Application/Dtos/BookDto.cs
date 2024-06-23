using Restaurants.Domain.Entities;

namespace Restaurants.Application.Dtos;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int NumberOfPages { get; set; }


    public static BookDto FromEntity(Book book)
    {
        return new()
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            NumberOfPages = book.NumberOfPages,
        };
    }
}
