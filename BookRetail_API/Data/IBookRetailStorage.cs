using BookRetail_API.Models;
using System.Collections.Generic;

namespace BookRetail_API.Data
{
    public interface IBookRetailStorage
    {
        int CountBooks();
        IEnumerable<Book> ListBooks();
        IEnumerable<Author> ListAuthors();
        IEnumerable<Publisher> ListPublishers();

        Book FindBook(string title);
        Author FindAuthor(string name);
        Publisher FindPublisher(string name);

        void CreateBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(string title);
    }
}