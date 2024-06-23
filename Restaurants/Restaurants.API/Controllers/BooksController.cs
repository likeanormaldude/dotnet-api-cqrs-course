using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Books;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBooksService booksService): ControllerBase
{
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
}
