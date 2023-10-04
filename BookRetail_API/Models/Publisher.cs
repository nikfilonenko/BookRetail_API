using System.Text.Json.Serialization;

namespace BookRetail_API.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }
        public string Name { get; set; }

        [JsonIgnore] public virtual ICollection<Book> Books { get; set; }
    }
}