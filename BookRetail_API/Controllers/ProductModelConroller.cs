using BookRetail_API.Data;
using BookRetail_API.HAL;
using BookRetail_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookRetail_API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductModelController : ControllerBase
{
    private readonly IBookRetailStorage _db;

    public ProductModelController(IBookRetailStorage db)
    {
        this._db = db;
    }

    [HttpGet]
    public IEnumerable<ProductModel> Get()
    {
        return _db.ListModels();
    }

    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var bookModel = _db.FindModel(id);
        if (bookModel == default) return NotFound();
        var resource = bookModel.ToDynamic();
        resource._actions = new
        {
            create = new
            {
                href = $"/api/books/{id}",
                type = "application/json",
                method = "POST",
                name = $"Create a new {bookModel.Publisher.Name} {bookModel.Code}"
            }
        };
        return Ok(resource);
    }
}