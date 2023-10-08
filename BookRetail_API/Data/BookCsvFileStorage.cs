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
            ReadManufacturersFromCsvFile("publishers.csv");
            ReadModelsFromCsvFile("models.csv");
            ReadVehiclesFromCsvFile("books.csv");
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

        private void ReadVehiclesFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var book = new Book {
                    Title = tokens[0],
                    ProductCode = tokens[1],
                    Price = Decimal.Parse(tokens[2]),
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
                    ManufacturerCode = tokens[1],
                    Name = tokens[2]
                };
                models.Add(model.Code, model);
            }
            logger.LogInformation($"Loaded {models.Count} models from {filePath}");
        }

        private void ReadManufacturersFromCsvFile(string filename) {
            var filePath = ResolveCsvFilePath(filename);
            foreach (var line in File.ReadAllLines(filePath)) {
                var tokens = line.Split(",");
                var mfr = new Manufacturer {
                    Code = tokens[0],
                    Name = tokens[1]
                };
                manufacturers.Add(mfr.Code, mfr);
            }
            logger.LogInformation($"Loaded {manufacturers.Count} manufacturers from {filePath}");
        }

        public int CountVehicles() => vehicles.Count;

        public IEnumerable<Vehicle> ListVehicles() => vehicles.Values;

        public IEnumerable<Manufacturer> ListManufacturers() => manufacturers.Values;

        public IEnumerable<Model> ListModels() => models.Values;

        public Vehicle FindVehicle(string registration) => vehicles.GetValueOrDefault(registration);

        public Model FindModel(string code) => models.GetValueOrDefault(code);

        public Manufacturer FindManufacturer(string code) => manufacturers.GetValueOrDefault(code);

        public void CreateVehicle(Vehicle vehicle) {
            vehicle.VehicleModel.Vehicles.Add(vehicle);
            vehicle.ModelCode = vehicle.VehicleModel.Code;
            UpdateVehicle(vehicle);
        }

        public void UpdateVehicle(Vehicle vehicle) {
            vehicles[vehicle.Registration] = vehicle;
        }

        public void DeleteVehicle(Vehicle vehicle) {
            var model = FindModel(vehicle.ModelCode);
            model.Vehicles.Remove(vehicle);
            vehicles.Remove(vehicle.Registration);
        }
    }