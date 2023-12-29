using Microsoft.AspNetCore.Mvc;
using BookRetail_API.Data;

namespace Book.WebSite.Controllers;

public class PublishersController : Controller {
    private readonly IBookRetailStorage _db;

    public PublishersController(IBookRetailStorage db) {
        this._db = db;
    }
    public IActionResult Index() {
        var books = _db.ListPublishers();
        return View(books);
    }

    public IActionResult Models(string id) {
        var publisher = _db.ListPublishers().FirstOrDefault(m => m.Code == id);
        return View(publisher);
    }
}