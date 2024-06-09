using NATS.Client;
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
            string serverCertificatePath = "../../certs/server-cert.pem";
            string serverCertificate = File.ReadAllText(serverCertificatePath);
            Console.WriteLine("Server Certificate:");
            Console.WriteLine(serverCertificate);
            opts.TLSRemoteCertificationValidationCallback = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return serverCertificate.Equals(certificate?.ToString());
            };
            opts.Url = "tls://localhost:4222"; // Update with NATS server URL using TLS

            // // Authentication configuration (if required)
            // opts.SetUserCredentials("username", "password"); // Update with your username and password

            using (var connection = new ConnectionFactory().CreateConnection(opts))
            {
                var subscription = connection.SubscribeAsync("event-topic");
                subscription.MessageHandler += (sender, msgArgs) =>
                {
                    var jsonMessage = Encoding.UTF8.GetString(msgArgs.Message.Data);
                    var eventMessage = JsonConvert.DeserializeObject<EventMessage>(jsonMessage);
                    Console.WriteLine("Received message:");
                    Console.WriteLine($"Message: {eventMessage!.Message}");
                };
                subscription.Start();

                Console.WriteLine("Subscriber is listening. Press any key to exit...");
                Console.ReadKey();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}