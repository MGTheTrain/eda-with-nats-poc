﻿using NATS.Client;
using Newtonsoft.Json;
using System;
using System.Text;

public class EventMessage
{
    public string? Message { get; set; }
}

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var opts = ConnectionFactory.GetDefaultOptions();
            opts.Secure = true;
            string caCertificatePath = "../../certs/ca-cert.pem";
            string caCertificate = File.ReadAllText(caCertificatePath);
            Console.WriteLine("CA Certificate:");
            Console.WriteLine(caCertificate);
            opts.TLSRemoteCertificationValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                // return caCertificate.Equals(certificate?.ToString()); // In production environments utilizing signed certificates from a Certificate Authority (CA), it's imperative to validate the certificate for secure communication
                return true; // bypass certificate validation with self-signed certs only
            };
            opts.Url = "tls://localhost:4222"; // Update with NATS server URL using TLS

            // Authentication configuration. Consider implementing more secure authentication methods beyond username and password for production environments.
            // Using TLS authentication with signed certificates may be adequate; the options mentioned above are a good starting point.
            // For more information check out https://docs.nats.io/running-a-nats-service/configuration/securing_nats/auth_intro/tls_mutual_auth
            opts.User = "test-user"; 
            opts.Password = "test-password";

            using (var connection = new ConnectionFactory().CreateConnection(opts))
            {
                var message = new EventMessage { Message = "Hello, NATS with TLS and authentication!" };
                var jsonMessage = JsonConvert.SerializeObject(message);
                var messageBytes = Encoding.UTF8.GetBytes(jsonMessage);
                connection.Publish("event-topic", messageBytes);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}