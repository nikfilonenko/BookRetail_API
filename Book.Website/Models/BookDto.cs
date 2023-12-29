using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Book.WebSite.Models;

public class BookDto
{
    [HiddenInput(DisplayValue = false)]
    public string ProductCode { get; set; }
    
    [Required]
    [DisplayName("Title")]
    public string Title { get; set; }

    [Required]
    [DisplayName("Author")]
    public string AuthorName { get; set; }

    [Required]
    [DisplayName("Publisher")]
    public string PublisherName { get; set; }

    [Required]
    [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
    [DisplayName("Publication Year")]
    public int PublicationYear { get; set; }
    
    public string Genre { get; set; }
    
    private static readonly string[] genres = new[]
    {
        "Science Fiction", "Mystery", "Romance", "Fantasy", "Thriller"
    };

    private static readonly SelectListItem blankSelectListItem = new SelectListItem("select...", String.Empty);
    public static IEnumerable<SelectListItem> ListGenres(string selectedGenre) {
        var items = new List<SelectListItem> { blankSelectListItem };
        items.AddRange(genres.Select(c => new SelectListItem(c, c, c == selectedGenre)));
        return items;
    }
}