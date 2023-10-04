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

        Book FindBook(int bookId);
        Author FindAuthor(int authorId);
        Publisher FindPublisher(int publisherId);

        void CreateBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int bookId);
    }
}
