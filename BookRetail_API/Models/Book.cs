using System.Text.Json.Serialization;

namespace BookRetail_API.Models
{
    public class Book
    {
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int PublisherId { get; set; }
        public decimal Price { get; set; }
        public int PublicationYear { get; set; }

        [JsonIgnore]
        public virtual Author Author { get; set; }

        [JsonIgnore]
        public virtual Publisher Publisher { get; set; }
    }
}