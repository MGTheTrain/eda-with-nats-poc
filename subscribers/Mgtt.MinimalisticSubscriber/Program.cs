using NATS.Client;
using Newtonsoft.Json;
using System.Text;

namespace Mgtt.MinimalisticSubscriber;

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
            opts.Url = "nats://localhost:4222";

            using (var connection = new ConnectionFactory().CreateConnection(opts))
            {
                var subscription = connection.SubscribeAsync("event-topic");
                subscription.MessageHandler += (sender, msgArgs) =>
                {
                    var jsonMessage = Encoding.UTF8.GetString(msgArgs.Message.Data);
                    var eventMessage = JsonConvert.DeserializeObject<EventMessage>(jsonMessage);
                    Console.WriteLine("Received message:");
                    Console.WriteLine($"Message: {eventMessage.Message}");
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
