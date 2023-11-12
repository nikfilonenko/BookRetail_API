using System.Text.Json.Serialization;

namespace BookRetail_API.Models
{
    public class Book
    {
        public string Title { get; set; }
        public string ProductCode { get; set; }
        public string Genre { get; set; }
        public string Author { get; set; }
        public int PublicationYear { get; set; }

        [JsonIgnore]
        public virtual ProductModel BookModel { get; set; }
    }
}