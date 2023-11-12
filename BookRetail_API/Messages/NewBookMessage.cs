namespace BookRetail_API.Messages;

public class NewBookMessage
{
    public string Title { get; set; } 
    public string Genre { get; set; } 
    public string Author { get; set; }
    public string PublisherName { get; set; } 
    public string ModelCode { get; set; }
    public int PublicationYear { get; set; } 
    public DateTimeOffset CreatedAt { get; set; }
}

public class NewBookPriceMessage : NewBookMessage
{
    public int Price { get; set; }
    
    public string CurrencyCode { get; set; }

    public NewBookPriceMessage() {}

    public NewBookPriceMessage(NewBookMessage book, int price, string currencyCode)
    {
        this.Title = book.Title;
        this.Genre = book.Genre;
        Author = book.Author;
        this.PublisherName = book.PublisherName;
        this.ModelCode = book.ModelCode;
        this.PublicationYear = book.PublicationYear;
        this.Price = price;
        this.CurrencyCode = currencyCode;
    }
}