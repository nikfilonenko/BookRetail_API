using BookRetail_API.Models;
using BookRetail_API.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using BookRetail_API.DTO_s;
using EasyNetQ;

namespace BookRetail_API.Controllers
{
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
        [Produces("application/json")]
        public IActionResult Get(int index = 0, int count = PAGE_SIZE)
        {
            var books = _db.ListBooks().Skip(index).Take(count);
            var total = _db.CountBooks();
            var result = new
            {
                count,
                total,
                index,
                items = books
            };
            return Ok(result);
        }

        // GET api/books/{title}
        [HttpGet("{title}")]
        [Produces("application/json")]
        public IActionResult Get(string title)
        {
            var book = _db.FindBook(title);
            if (book == null) return NotFound();
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post([FromBody] BookDto bookDto)
        {
            // Create a new book object from the DTO
            var newBook = new Book
            {
                Title = bookDto.Title,
                Author = _db.FindAuthor(bookDto.AuthorName) ?? new Author { Name = bookDto.AuthorName }, // Find or create author
                Publisher = _db.FindPublisher(bookDto.PublisherName) ?? new Publisher { Name = bookDto.PublisherName }, // Find or create publisher
                Price = bookDto.Price,
                PublicationYear = bookDto.PublicationYear
            };

            // Create the book in the storage
            _db.CreateBook(newBook);

            // Return a response with the created book
            return Created($"/api/books/{newBook.Title}", newBook);
        }

        // PUT api/books/{title}
        [HttpPut("{title}")]
        public IActionResult Put(string title, [FromBody] BookDto bookDto)
        {
            var existingBook = _db.FindBook(title);
            if (existingBook == null) return NotFound();

            // Update the book properties based on the DTO
            existingBook.Title = bookDto.Title;
            existingBook.Author = _db.FindAuthor(bookDto.AuthorName) ?? new Author { Name = bookDto.AuthorName };
            existingBook.Publisher = _db.FindPublisher(bookDto.PublisherName) ?? new Publisher { Name = bookDto.PublisherName };
            existingBook.Price = bookDto.Price;
            existingBook.PublicationYear = bookDto.PublicationYear;

            // Update the book in the storage
            _db.UpdateBook(existingBook);

            // Return a response with the updated book
            return Ok(existingBook);
        }

        // DELETE api/books/{title}
        [HttpDelete("{title}")]
        public IActionResult Delete(string title)
        {
            var existingBook = _db.FindBook(title);
            if (existingBook == null) return NotFound();

            // Delete the book from the storage
            _db.DeleteBook(title);

            return NoContent();
        }
        
        private async Task PublishNewVehicleMessage(Book book) {
            var message = book.ToMessage();
            await _bus.PubSub.PublishAsync(message);
        }
    }
}
