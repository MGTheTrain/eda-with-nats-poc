﻿using NATS.Client;
using Newtonsoft.Json;
using System;
using System.Text;

namespace Mgtt.MinimalisticPublisher;
public class EventMessage
{
    public string? Message { get; set; }
}
public class Program
{
    public static void Main(string[] args)
    {
        var opts = ConnectionFactory.GetDefaultOptions();
        opts.Url = "nats://localhost:4222";

        using (var connection = new ConnectionFactory().CreateConnection(opts))
        {
            var message = new EventMessage { Message = "Hello NATS" };
            var jsonMessage = JsonConvert.SerializeObject(message);
            var messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
            connection.Publish("event-topic", messageBytes);
        }
    }
}
