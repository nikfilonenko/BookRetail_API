using BookRetail_API.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BookRetail_API
{
    public class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();
        
        public static void Main(string[] args)
        {
            var amqp = config.GetConnectionString("AutoRabbitMQ");
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for NewBookMessages");
            var subscriberId = $"Book.AuditLog@{Environment.MachineName}";
            bus.PubSub.SubscribeAsync<NewBookMessage>(subscriberId, HandleNewBookMessage);
            Console.ReadLine();
        }

        private static void HandleNewBookMessage(NewBookMessage nvm) {
            var csvRow =
                $"{nvm.Title},{nvm.Genre},{nvm.Author},{nvm.PublisherName},{nvm.PublicationYear},{nvm.ModelCode},{nvm.CreatedAt:O}";
            Console.WriteLine(csvRow);
        }
        
        private static IConfigurationRoot ReadConfiguration() {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}