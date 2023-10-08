namespace BookRetail_API.Messages;

public class NewBookMessage
{
    public string Title { get; set; } 
    public string Genre { get; set; } 
    public string PublisherName { get; set; } 
    public string ModelCode { get; set; }
    public string Price { get; set; } 
    public int PublicationYear { get; set; } 
    public DateTimeOffset CreatedAt { get; set; }
}