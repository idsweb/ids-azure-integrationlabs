using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;

namespace Lab_servicebus
{
    class Program
    {
        static string connectionString = 
        static string queueName = "dupequeue";
        static async Task Main(string[] args)
        {
            await SendMessageAsync();
            Console.WriteLine("Done");
        }

        static async Task SendMessageAsync()
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                ServiceRequestGenerator generator = new ServiceRequestGenerator();

                var req = generator.CreateRequest();

                string jsonReq = JsonConvert.SerializeObject(req);

                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonReq))
                {
                    ContentType = "application/json",
                    MessageId = String.Format("{0}/abandonedvehicle", Guid.NewGuid().ToString())
                };

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }
    }

}
