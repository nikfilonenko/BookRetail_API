using BookRetail_API.Data;
using BookRetail_API.DTO_s;
using BookRetail_API.HAL;
using BookRetail_API.Models;
using EasyNetQ;
using Microsoft.AspNetCore.Mvc;

namespace BookRetail_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly IBookRetailStorage _db;
    private readonly IBus _bus;

    public BooksController(IBookRetailStorage db, IBus bus)
    {
        this._db = db;
        this._bus = bus;
    }

    const int PAGE_SIZE = 25;

    // GET: api/books
    [HttpGet]
    [Produces("application/hal+json")]
    public IActionResult Get(int index = 0, int count = PAGE_SIZE)
    {
        var items = _db.ListBooks().Skip(index).Take(count)
            .Select(v => v.ToResource());
        var total = _db.CountBooks();
        var _links = HAL.HAL.PaginateAsDynamic("/api/books", index, count, total);
        var result = new
        {
            _links,
            count,
            total,
            index,
            items
        };
        return Ok(result);
    }

    // GET api/books/ABC123
    [HttpGet("{id}")]
    [Produces("application/hal+json")]
    public IActionResult Get(string id)
    {
        var book = _db.FindBook(id);
        if (book == null) return NotFound();
        var resource = book.ToResource();
        resource._actions = new
        {
            delete = new
            {
                href = $"/api/book/{id}",
                method = "DELETE",
                name = $"Delete {id} from the database"
            }
        };
        return Ok(resource);
    }

    // PUT api/books/ABC123
    [HttpPut("{id}")]
    public IActionResult Put(string id, [FromBody] BookDto dto)
    {
        var bookModel = _db.FindModel(dto.ProductCode);
        var book = _db.FindBook(id);

        book.Title = dto.Title;
        book.ProductCode = bookModel.Code;
        book.Genre = dto.Genre;
        book.PublicationYear = dto.PublicationYear;
        book.Author = dto.AuthorName;
        
        _db.UpdateBook(book);
        return Ok(book);
    }
    
    // POST api/books
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] BookDto dto)
    {
        var existing = _db.FindBook(dto.Title);
        if (existing != default)
            return Conflict($"Sorry, there is already a book with title {dto.Title} in the database.");
        var bookModel = _db.FindModel(dto.ProductCode);
        if (bookModel == default)
            return Conflict($"Sorry, there is already a book with title {dto.ProductCode} in the database.");
        
        var book = new Book
        {
            Title = dto.Title,
            Genre = dto.Genre,
            PublicationYear = dto.PublicationYear,
            BookModel = bookModel,
            Author = dto.AuthorName
        };
        _db.CreateBook(book);
        await PublishNewBookMessage(book);
        return Created($"/api/books/{book.Title}", book.ToResource());
    }

    // DELETE api/books/ABC123
    [HttpDelete("{id}")]
    public IActionResult Delete(string id)
    {
        var book = _db.FindBook(id);
        if (book == default) return NotFound();
        _db.DeleteBook(book);
        return NoContent();
    }
    
    private async Task PublishNewBookMessage(Book book) {
        var message = book.ToMessage();
        await _bus.PubSub.PublishAsync(message);
    }
}