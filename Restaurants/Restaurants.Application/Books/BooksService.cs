using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Books;

public class BooksService(IRestaurantsRepository restaurantsRepository, ILogger<BooksService> logger) : IBooksService
{
    public async Task<IEnumerable<BookDto>> GetAllBooks()
    {
        logger.LogInformation("Getting all books");
        
        var books = await restaurantsRepository.GetAllBooksAsync();
        var booksDto = books.Select(r => BookDto.FromEntity(r));

        return booksDto;
    }
}
