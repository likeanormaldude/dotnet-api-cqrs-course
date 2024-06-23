using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dtos;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Books;

public class BooksService(
    IRestaurantsRepository restaurantsRepository,
    ILogger<BooksService> logger,
    IMapper mapper
) : IBooksService
{
    public async Task<IEnumerable<BookDto>> GetAllBooks()
    {
        logger.LogInformation("Getting all books");
        
        var books = await restaurantsRepository.GetAllBooksAsync();
        var booksDto = mapper.Map<IEnumerable<BookDto>>(books);

        return booksDto;
    }

    public async Task<BookDto?> GetBookById(int id)
    {
        logger.LogInformation($"Getting book with id {id}");

        var book = await restaurantsRepository.GetBookByIdAsync(id);

        if(book == null )
            return null;

        var bookDto = mapper.Map<BookDto>(book);

        return bookDto;
    }

    public async Task<IEnumerable<BookDto>> CreateBook(Book book)
    {
        logger.LogInformation("Creating new book");
        
        var updatedBookList = await restaurantsRepository.CreateBook(book);
        var updatedBookDtoList = mapper.Map<IEnumerable<BookDto>>(updatedBookList);
        
        return updatedBookDtoList;
    }

    public async Task<IEnumerable<BookDto>?> DeleteBook(int id)
    {
        logger.LogInformation($"Deleting book with id{id}");
        var updatedBookList = await restaurantsRepository.DeleteBook(id);

        if (updatedBookList == null)
            return null;

        var updatedBookDtoList = mapper.Map<IEnumerable<BookDto>>(updatedBookList);

        return updatedBookDtoList;
    }

    public async Task<IEnumerable<BookDto>?> UpdateBook(Book newBook)
    {
        logger.LogInformation("Updating new book");

        var updatedBookList = await restaurantsRepository.UpdateBook(newBook);

        if (updatedBookList == null)
            return null;

        var updatedBookDtoList = mapper.Map<IEnumerable<BookDto>>(updatedBookList);

        return updatedBookDtoList;
    }
}
