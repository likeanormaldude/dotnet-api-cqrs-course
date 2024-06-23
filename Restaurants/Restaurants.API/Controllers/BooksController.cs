using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Books;
using Restaurants.Domain.Entities;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBooksService booksService): ControllerBase
{
    // .https://localhost:7255/api/books
    [HttpGet]
    public async Task<IActionResult> GetAllBooks()
    {
        var books = await booksService.GetAllBooks();

        if(books == null)
        {
            return NotFound();
        }

        return Ok(books);
    }

    // .https://localhost:7255/api/books/12
    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookById([FromRoute] int id)
    {
        var books = await booksService.GetBookById(id);

        if (books == null)
        {
            return NotFound();
        }

        return Ok(books);
    }

    // .https://localhost:7255/api/books/
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] Book book)
    {
        var books = await booksService.CreateBook(book);

        if (books == null)
        {
            return StatusCode(500, "Could not create Book.");
        }

        return Ok(books);
    }

    // .https://localhost:7255/api/books/12
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook([FromRoute] int id)
    {
        var books = await booksService.DeleteBook(id);

        if (books == null)
        {
            return NotFound("Book not found.");
        }

        return Ok(books);
    }

    // .https://localhost:7255/api/books/
    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] Book newBook)
    {
        var updatedBookList = await booksService.UpdateBook(newBook);

        if (updatedBookList == null)
        {
            return NotFound("Book not found in order to be updated.");
        }

        return Ok(updatedBookList);
    }


}
