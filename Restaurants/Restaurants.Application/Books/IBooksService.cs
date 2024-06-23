using Restaurants.Application.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Books;

public interface IBooksService
{
    Task<IEnumerable<BookDto>> GetAllBooks();
    Task<BookDto?> GetBookById(int id);
    Task<IEnumerable<BookDto>> CreateBook(Book book);
    Task<IEnumerable<BookDto>?> UpdateBook(Book newBook);
    Task<IEnumerable<BookDto>?> DeleteBook(int id);
}
