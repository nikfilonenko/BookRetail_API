using BookRetail_API.Data;
using Microsoft.AspNetCore.Mvc;

namespace Book.WebSite.Controllers;

public class ModelsController : Controller {
    private readonly IBookRetailStorage _db;

    public ModelsController(IBookRetailStorage db) {
        this._db = db;
    }

    public IActionResult Books(string id) {
        var model = _db.ListModels().FirstOrDefault(m => m.Code == id);
        return View(model);
    }

    public IActionResult Index() {
        var models = _db.ListModels();
        return View(models);
    }
}