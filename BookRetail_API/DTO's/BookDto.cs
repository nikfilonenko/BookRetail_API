using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookRetail_API.DTO_s;

public class BookDto
{
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
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    [DisplayName("Price")]
    public decimal Price { get; set; }

    [Required]
    [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100")]
    [DisplayName("Publication Year")]
    public int PublicationYear { get; set; }
}