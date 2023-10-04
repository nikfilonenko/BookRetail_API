using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BookRetail_API.Models;
using Microsoft.Extensions.Logging;

namespace BookRetail_API.Data
{
    public class BookCsvFileStorage : IBookRetailStorage
    {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Publisher> publishers = new Dictionary<string, Publisher>(collation);
        private readonly Dictionary<string, Author> authors = new Dictionary<string, Author>(collation);
        private readonly Dictionary<int, Book> books = new Dictionary<int, Book>();
        private readonly ILogger<BookCsvFileStorage> logger;

        public BookCsvFileStorage(ILogger<BookCsvFileStorage> logger)
        {
            this.logger = logger;
            ReadPublishersFromCsvFile("publishers.csv");
            ReadAuthorsFromCsvFile("authors.csv");
            ReadBooksFromCsvFile("books.csv");
            ResolveReferences();
        }

        private void ResolveReferences()
        {
            foreach (var book in books.Values)
            {
                book.Author = authors.GetValueOrDefault(book.AuthorId);
                book.Publisher = publishers.GetValueOrDefault(book.PublisherId);
            }
        }

        private string ResolveCsvFilePath(string filename)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "csv-data");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadBooksFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                if (int.TryParse(tokens[0], out var bookId))
                {
                    var book = new Book
                    {
                        BookId = bookId,
                        Title = tokens[1],
                        AuthorId = int.Parse(tokens[2]),
                        PublisherId = int.Parse(tokens[3]),
                        Price = decimal.Parse(tokens[4]),
                        PublicationYear = int.Parse(tokens[5])
                    };
                    books.Add(book.BookId, book);
                }
            }
            logger.LogInformation($"Loaded {books.Count} books from {filePath}");
        }

        private void ReadAuthorsFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                if (int.TryParse(tokens[0], out var authorId))
                {
                    var author = new Author
                    {
                        AuthorId = authorId,
                        Name = tokens[1]
                    };
                    authors.Add(author.AuthorId, author);
                }
            }
            logger.LogInformation($"Loaded {authors.Count} authors from {filePath}");
        }

        private void ReadPublishersFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                if (int.TryParse(tokens[0], out var publisherId))
                {
                    var publisher = new Publisher
                    {
                        PublisherId = publisherId,
                        Name = tokens[1]
                    };
                    publishers.Add(publisher.PublisherId, publisher);
                }
            }
            logger.LogInformation($"Loaded {publishers.Count} publishers from {filePath}");
        }

        public int CountBooks() => books.Count;

        public IEnumerable<Book> ListBooks() => books.Values;

        public IEnumerable<Author> ListAuthors() => authors.Values;

        public IEnumerable<Publisher> ListPublishers() => publishers.Values;

        public Book FindBook(int bookId) => books.GetValueOrDefault(bookId);

        public Author FindAuthor(int authorId) => authors.GetValueOrDefault(authorId);

        public Publisher FindPublisher(int publisherId) => publishers.GetValueOrDefault(publisherId);

        public void CreateBook(Book book)
        {
            books.Add(book.BookId, book);
        }

        public void UpdateBook(Book book)
        {
            if (books.ContainsKey(book.BookId))
            {
                books[book.BookId] = book;
            }
        }

        public void DeleteBook(int bookId)
        {
            books.Remove(bookId);
        }
    }
}
