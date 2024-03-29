﻿using System.Text.Json;
using BookRetail_API.Messages;
using EasyNetQ;
using Microsoft.AspNetCore.SignalR.Client;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Book.Notifier
{
    internal class Program
    {
        const string SIGNALR_HUB_URL = "https://localhost:7274/hub";
        private static HubConnection hub;

        static async Task Main(string[] args)
        {
            hub = new HubConnectionBuilder().WithUrl(SIGNALR_HUB_URL).Build();
            await hub.StartAsync();
            Console.WriteLine("Hub started!");
            Console.WriteLine("Press any key to send a message (Ctrl-C to quit)");
            var amqp = "amqp://user:rabbitmq@localhost:5672";
            using var bus = RabbitHutch.CreateBus(amqp);
            Console.WriteLine("Connected to bus! Listening for newBookMessages");
            var subscriberId = $"Book.Notifier@{Environment.MachineName}";
            await bus.PubSub.SubscribeAsync<NewBookMessage>(subscriberId, HandleNewBookMessage);
            Console.ReadLine();
        }

        private static async void HandleNewBookMessage(NewBookMessage nbpm)
        {
            var csvRow =
                $"{nbpm.Title},{nbpm.PublisherName}," +
                $"{nbpm.ModelCode},{nbpm.PublicationYear},{nbpm.Genre},{nbpm.Author},{nbpm.CreatedAt:O}";
            Console.WriteLine(csvRow);
            var json = JsonSerializer.Serialize(nbpm, JsonSettings());
            await hub.SendAsync("NotifyWebUsers", "Book.Notifier",
                json);
        }

        static JsonSerializerOptions JsonSettings() => new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }
}