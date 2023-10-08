using Newtonsoft.Json;

namespace BookRetail_API.Models;

public class ProductModel
{
    public ProductModel() {
        Books = new HashSet<Book>();
    }

    public string Code { get; set; }
    public string PublisherCode { get; set; }

    public virtual Publisher Publisher { get; set; }
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; }
}