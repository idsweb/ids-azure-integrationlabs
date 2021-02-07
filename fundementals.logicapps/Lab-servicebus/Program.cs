using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Hosting;
using Lab_servicebus.Models;
using System.IO;

namespace Lab_servicebus
{
    class Program
    {
        static string queueName = "dupequeue";
        static async Task Main(string[] args)
        {
            await SendMessageAsync();
            Console.WriteLine("Done");
        }

        static async Task SendMessageAsync()
        {
            string messageId = Guid.NewGuid().ToString();
            // sas is kept in a seperate json file not checked in.
            Secrets secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(@"secrets.json"));
            string connectionString = secrets.ServiceBusConnectionString;

            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                ServiceRequestGenerator generator = new ServiceRequestGenerator();

                var req = await generator.CreateRequest();

                string jsonReq = JsonConvert.SerializeObject(req);

                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonReq))
                {
                    ContentType = "application/json",
                    MessageId = String.Format("{0}/abandonedvehicle", messageId)
                };

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");

            }
        }
    }

}
