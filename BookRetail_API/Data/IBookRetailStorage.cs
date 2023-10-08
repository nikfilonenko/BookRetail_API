using BookRetail_API.Models;
using System.Collections.Generic;

namespace BookRetail_API.Data
{
    public interface IBookRetailStorage
    {
        public int CountBooks();
        public IEnumerable<Book> ListBooks();
        public IEnumerable<ProductModel> ListModels();
        public IEnumerable<Publisher> ListPublishers();

        public Book FindBook(string title);
        public ProductModel FindAuthor(string code);
        public Publisher FindPublisher(string code);

        public void CreateBook(Book book);
        public void UpdateBook(Book book);
        public void DeleteBook(Book book);
    }
}
