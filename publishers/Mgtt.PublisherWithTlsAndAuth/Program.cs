﻿using NATS.Client;
using Newtonsoft.Json;
using System;
using System.Text;

public class EventMessage
{
    public string Message { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Secure = true;
            opts.TLSRemoteCertificationValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true; // Disable certificate validation for self-signed certificates
            opts.Url = "tls://localhost:4222"; // Update with NATS server URL using TLS

            // Authentication configuration (if required)
            opts.SetUserCredentials("username", "password"); // Update with your username and password

            using (var connection = new ConnectionFactory().CreateConnection(opts))
            {
                var message = new EventMessage { Message = "Hello, NATS with TLS and authentication!" };
                var jsonMessage = JsonConvert.SerializeObject(message);
                var messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
                connection.Publish("event-topic", messageBytes);

                Console.WriteLine("Message published. Press any key to exit...");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}