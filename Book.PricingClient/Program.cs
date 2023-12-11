using Book.PricingEngine;
using BookRetail_API.Messages;
using EasyNetQ;
using Grpc.Net.Client;

namespace Book.PricingClient {
    class Program {
        private static Pricer.PricerClient grpcClient;
        private static IBus bus;
        static async Task Main(string[] args) {
            Console.WriteLine("Starting Book.PricingClient");
            
            var amqp ="amqp://user:rabbitmq@localhost:5672";
            bus = RabbitHutch.CreateBus(amqp);
            
            Console.WriteLine("Connected to bus; Listening for newBookMessages");
            
            var grpcAddress = "https://localhost:7277";
            using var channel = GrpcChannel.ForAddress(grpcAddress);
            grpcClient = new Pricer.PricerClient(channel);
            
            Console.WriteLine($"Connected to gRPC on {grpcAddress}!");
            
            var subscriberId = $"Book.PricingClient@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewBookMessage>(subscriberId, HandleNewBookMessage);
            
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }

        private static async Task HandleNewBookMessage(NewBookMessage message) {
            Console.WriteLine($"new book; {message.Title}");
            var priceRequest = new PriceRequest() {
                Genre = message.Genre,
                Author = message.Author,
                PublisherName = message.PublisherName,
                Year = message.PublicationYear,
                ModelCode = message.ModelCode
            };
            var priceReply = await grpcClient.GetPriceAsync(priceRequest);
            Console.WriteLine($"Book {message.Title} costs {priceReply.Price} {priceReply.CurrencyCode}");
            var newVehiclePriceMessage = new NewBookPriceMessage(message, priceReply.Price, priceReply.CurrencyCode);
            await bus.PubSub.PublishAsync(newVehiclePriceMessage);
        }
    }
}