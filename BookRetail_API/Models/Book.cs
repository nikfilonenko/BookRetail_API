namespace BookRetail_API.Models;

public class Book
{
    public int BookId { get; set; }
    public string Title { get; set; }
    public string PublishingHouse { get; set; }
    public decimal Price { get; set; }
    public DateTime ReleaseDate { get; set; }
}