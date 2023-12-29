using BookRetail_API.Models;
using BookRetail_API.Data;
using Book.WebSite.Models;
using Microsoft.AspNetCore.Mvc;

namespace Book.WebSite.Controllers;

public class BooksController : Controller {
    private readonly IBookRetailStorage _db;

    public BooksController(IBookRetailStorage db) {
        this._db = db;
    }
    
    public IActionResult Index() {
        var books = _db.ListBooks();
        
        return View(books);
    }

    public IActionResult Details(string id) {
        var books = _db.FindBook(id);
        
        return View(books);
    }

    [HttpGet]
    public IActionResult Advertise(string id) {
        var booksModel = _db.FindModel(id);
        
        var dto = new BookDto() {
            ProductCode = booksModel.Code,
            PublisherName = $"{booksModel.Publisher.Name} {booksModel.Code}"
        };
        return View(dto);
    }

    [HttpPost]
    public IActionResult Advertise(BookDto dto) {
        var existingBook = _db.FindBook(dto.Title);
        
        if (existingBook != default)
            ModelState.AddModelError(nameof(dto.Title),
                "That title is already listed in our database.");
        var bookModel = _db.FindModel(dto.ProductCode);
        
        if (bookModel == default) {
            ModelState.AddModelError(nameof(dto.ProductCode),
                $"Sorry, {dto.ProductCode} is not a valid model code.");
        }
        if (!ModelState.IsValid) return View(dto);
        
        var book = new BookRetail_API.Models.Book() {
            Title = dto.Title,
            Genre = dto.Genre,
            BookModel = bookModel,
            PublicationYear = dto.PublicationYear
        };
        
        _db.CreateBook(book);
        return RedirectToAction("Details", new { id = book.Title });
    }
}