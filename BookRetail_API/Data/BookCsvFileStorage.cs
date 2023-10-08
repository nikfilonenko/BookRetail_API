using System.Reflection;
using BookRetail_API.Models;
using static System.Int32;
using Microsoft.Extensions.Logging;

namespace BookRetail_API.Data;

public class BookCsvFileStorage : IBookRetailStorage {
        private static readonly IEqualityComparer<string> collation = StringComparer.OrdinalIgnoreCase;

        private readonly Dictionary<string, Publisher> publishers = new Dictionary<string, Publisher>(collation);
        private readonly Dictionary<string, ProductModel> models = new Dictionary<string, ProductModel>(collation);
        private readonly Dictionary<string, Book> books = new Dictionary<string, Book>(collation);
        private readonly ILogger<BookCsvFileStorage> logger;

        public BookCsvFileStorage(ILogger<BookCsvFileStorage> logger) {
            this.logger = logger;
            ReadPublishersFromCsvFile("publishers.csv");
            ReadModelsFromCsvFile("models.csv");
            ReadBooksFromCsvFile("books.csv");
            ResolveReferences();
        }

        private void ResolveReferences() {
            foreach (var pbsh in publishers.Values) {
                pbsh.Models = models.Values.Where(m => m.PublisherCode == pbsh.Code).ToList();
                foreach (var model in pbsh.Models) model.Publisher = pbsh;
            }

            foreach (var model in models.Values) {
                model.Books = books.Values.Where(v => v.ProductCode == model.Code).ToList();
                foreach (var vehicle in model.Books) vehicle.BookModel = model;
            }
        }

        private string ResolveCsvFilePath(string filename) {
            return Path.Combine("C:\\Users\\79057\\RiderProjects\\BookRetail_API\\BookRetail_API\\Data", filename);
        }

        private void ReadBooksFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var book = new Book {
                    Title = tokens[0],
                    ProductCode = tokens[1],
                    Price = tokens[2],
                    Genre = tokens[3],
                    Author = tokens[4]
                };
                if (TryParse(tokens[5], out var year)) book.PublicationYear = year;
                books[book.Title] = book;
            }
            logger.LogInformation($"Loaded {books.Count} books from {filePath}");
        }

        private void ReadModelsFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var model = new ProductModel {
                    Code = tokens[0],
                    PublisherCode = tokens[1],
                };
                models.Add(model.Code, model);
            }
            logger.LogInformation($"Loaded {models.Count} models from {filePath}");
        }

        private void ReadPublishersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var pbsh = new Publisher {
                    Code = tokens[0],
                    Name = tokens[1]
                };
                publishers.Add(pbsh.Code, pbsh);
            }
            logger.LogInformation($"Loaded {publishers.Count} publishers from {filePath}");
        }

        public int CountBooks() => books.Count;

        public IEnumerable<Book> ListBooks() => books.Values;

        public IEnumerable<Publisher> ListPublishers() => publishers.Values;

        public IEnumerable<ProductModel> ListModels() => models.Values;

        public Book FindBook(string title) => books.GetValueOrDefault(title);

        public ProductModel FindModel(string code) => models.GetValueOrDefault(code);

        public Publisher FindPublisher(string code) => publishers.GetValueOrDefault(code);

        public void CreateBook(Book book) {
            book.BookModel.Books.Add(book);
            book.ProductCode = book.BookModel.Code;
            UpdateBook(book);
        }

        public void UpdateBook(Book book) {
            books[book.Title] = book;
        }

        public void DeleteBook(Book book) {
            var model = FindModel(book.ProductCode);
            model.Books.Remove(book);
            books.Remove(book.Title);
        }
    }