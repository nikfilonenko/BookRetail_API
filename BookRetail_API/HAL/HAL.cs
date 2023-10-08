using System.ComponentModel;
using System.Dynamic;
using BookRetail_API.Messages;
using BookRetail_API.Models;

namespace Auto.API;
public static class HAL
{
        public static dynamic PaginateAsDynamic(string baseUrl, int index, int count, int total) {
                dynamic links = new ExpandoObject();
                links.self = new { href = "/api/books" };
                if (index < total) {
                    links.next = new { href = $"/api/books?index={index + count}" };
                    links.final = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
                }
                if (index > 0) {
                    links.prev = new { href = $"/api/books?index={index - count}" };
                    links.first = new { href = $"/api/books?index=0" };
                }
                return links;
        }

        public static Dictionary<string, object> PaginateAsDictionary(string baseUrl, int index, int count, int total) {
            var links = new Dictionary<string, object>();
            links.Add("self", new { href = "/api/books" });
            if (index < total) {
                links["next"] = new { href = $"/api/books?index={index + count}" };
                links["final"] = new { href = $"{baseUrl}?index={total - (total % count)}&count={count}" };
            }
            if (index > 0) {
                links["prev"] = new { href = $"/api/books?index={index - count}" };
                links["first"] = new { href = $"/api/books?index=0" };
            }
            return links;
        }

        public static dynamic ToResource(this Book book) {
            var resource = book.ToDynamic();
            resource._links = new {
                self = new {
                    href = $"/api/books/{book.Title}"
                },
                model = new {
                    href = $"/api/models/{book.ProductCode}"
                }
            };
            return resource;
        }

        public static dynamic ToDynamic(this object value) {
            IDictionary<string, object> result = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor prop in properties) {
                if (Ignore(prop)) continue;
                result.Add(prop.Name, prop.GetValue(value));
            }
            return result;
        }

        private static bool Ignore(PropertyDescriptor prop) {
            return prop.Attributes.OfType<System.Text.Json.Serialization.JsonIgnoreAttribute>().Any();
        }
        
        public static NewBookMessage ToMessage(this Book book) {
            var message = new NewBookMessage {
                Title = book.Title,
                Genre = book.Genre,
                PublisherName = book?.BookModel?.Publisher?.Name,
                ModelCode = book?.BookModel?.Code,
                Price = book.Price,
                PublicationYear = book.PublicationYear,
                CreatedAt = DateTimeOffset.UtcNow,
            };
            return message;
        }
}