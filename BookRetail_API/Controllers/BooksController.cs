using BookRetail_API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BookRetail_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // GET: api/books
        [HttpGet]
        [Produces("application/json")]
        public IActionResult Get()
        {
            var books = _bookRepository.GetAllBooks();
            return Ok(books);
        }

        // GET api/books/1
        [HttpGet("{id}")]
        [Produces("application/json")]
        public IActionResult Get(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }

        // POST api/books
        [HttpPost]
        public IActionResult Post([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }
            _bookRepository.AddBook(book);
            return CreatedAtAction("Get", new { id = book.BookId }, book);
        }

        // PUT api/books/1
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Book book)
        {
            if (book == null || id != book.BookId)
            {
                return BadRequest();
            }
            var existingBook = _bookRepository.GetBookById(id);
            if (existingBook == null)
            {
                return NotFound();
            }
            _bookRepository.UpdateBook(book);
            return NoContent();
        }

        // DELETE api/books/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var book = _bookRepository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }
            _bookRepository.DeleteBook(id);
            return NoContent();
        }
    }
}
