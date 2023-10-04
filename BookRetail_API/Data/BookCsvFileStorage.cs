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
        private readonly Dictionary<string, Book> books = new Dictionary<string, Book>();

        private readonly ILogger<BookCsvFileStorage> logger;

        public BookCsvFileStorage(ILogger<BookCsvFileStorage> logger)
        {
            this.logger = logger;
            ReadPublishersFromCsvFile("books.csv");
            ReadAuthorsFromCsvFile("books.csv");
            ReadBooksFromCsvFile("books.csv");
            ResolveReferences();
        }

        private void ResolveReferences()
        {
            foreach (var book in books.Values)
            {
                book.Author = authors.GetValueOrDefault(book.Author.Name);
                book.Publisher = publishers.GetValueOrDefault(book.Publisher.Name);
            }
        }

        private string ResolveCsvFilePath(string filename)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var csvFilePath = Path.Combine(directory, "Csv files");
            return Path.Combine(csvFilePath, filename);
        }

        private void ReadBooksFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var book = new Book
                {
                    Title = tokens[0],
                    Author = new Author { Name = tokens[1] }, // Создаем автора на основе имени
                    Publisher = new Publisher { Name = tokens[2] }, // Создаем издателя на основе имени
                    Price = decimal.Parse(tokens[3]),
                    PublicationYear = int.Parse(tokens[4])
                };
                books[book.Title] = book;
            }
            logger.LogInformation($"Loaded {books.Count} books from {filePath}");
        }

        private void ReadAuthorsFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var author = new Author
                {
                    Name = tokens[0]
                };
                authors[author.Name] = author;
            }
            logger.LogInformation($"Loaded {authors.Count} authors from {filePath}");
        }

        private void ReadPublishersFromCsvFile(string filename)
        {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath))
            {
                var tokens = line.Split(",");
                var publisher = new Publisher
                {
                    Name = tokens[0]
                };
                publishers[publisher.Name] = publisher;
            }
            logger.LogInformation($"Loaded {publishers.Count} publishers from {filePath}");
        }

        public int CountBooks() => books.Count;

        public IEnumerable<Book> ListBooks() => books.Values;

        public IEnumerable<Author> ListAuthors() => authors.Values;

        public IEnumerable<Publisher> ListPublishers() => publishers.Values;

        public Book FindBook(string title) => books.GetValueOrDefault(title);

        public Author FindAuthor(string name) => authors.GetValueOrDefault(name);

        public Publisher FindPublisher(string name) => publishers.GetValueOrDefault(name);

        public void CreateBook(Book book)
        {
            books[book.Title] = book;
        }

        public void UpdateBook(Book book)
        {
            if (books.ContainsKey(book.Title))
            {
                books[book.Title] = book;
            }
        }

        public void DeleteBook(string title)
        {
            books.Remove(title);
        }
    }
}
