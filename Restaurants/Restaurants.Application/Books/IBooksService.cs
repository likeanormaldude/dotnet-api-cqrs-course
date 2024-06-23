using Restaurants.Application.Dtos;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Books;

public interface IBooksService
{
    Task<IEnumerable<BookDto>> GetAllBooks();
}
