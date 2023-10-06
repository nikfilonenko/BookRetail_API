using Microsoft.AspNetCore.Mvc;

namespace BookRetail_API.API.Controllers;

[Route("api")]
[ApiController]
public class DiscoveryEndpointController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() {
        var welcome = new {
            _links = new {
                books = new {
                    href = "/api/booksretail"
                }
            },
            message = "Welcome to the BooksRetail API!",
        };
        return Ok(welcome);
    }
}