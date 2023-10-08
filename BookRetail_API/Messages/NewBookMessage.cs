namespace BookRetail_API.Messages;

public class NewBookMessage
{
    public string Title { get; set; } 
    public string AuthorName { get; set; } 
    public string PublisherName { get; set; } 
    public decimal Price { get; set; } 
    public int PublicationYear { get; set; } 
    public DateTimeOffset CreatedAt { get; set; }
}