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
            opts.User = "test-user";
            opts.Password = "test-password";

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