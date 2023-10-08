using System.Text.Json.Serialization;

namespace BookRetail_API.Models
{
    public class Publisher
    {
        public Publisher()
        {
            Models = new HashSet<ProductModel>();
        }
        public string Code { get; set; }
        public string Name { get; set; }

        [JsonIgnore] 
        public virtual ICollection<ProductModel> Models { get; set; }
    }
}