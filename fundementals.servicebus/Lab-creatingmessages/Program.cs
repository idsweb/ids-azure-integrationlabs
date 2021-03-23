using System;
using System.Threading;
using System.Threading.Tasks;
using Lab_creatingmessages.Models;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using Azure.Messaging.ServiceBus;

// https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues

namespace Lab_creatingmessages
{
    class Program
    {

        static async Task Main(string[] args)
        {
            Secrets secrets = JsonConvert.DeserializeObject<Secrets>(File.ReadAllText(@"secrets.json"));
            string connectionString = secrets.ServiceBusConnectionString;
            string queuename = "myqueue";
            await SendMessageAsync(connectionString, queuename);
            
            // now read the message back
            // you need two methods - one to handle errors and another to handle messages
            

        }

        private static async Task ReceiveMessageAsync(string connectionString, string queueName)
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                
                // create a sender for the queue 
                ServiceBusReceiver receiver = client.CreateReceiver(queueName, new ServiceBusReceiverOptions(){
                    ReceiveMode = ServiceBusReceiveMode.PeekLock
                });

                // receive the message
                
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }

        static async Task SendMessageAsync(string connectionString, string queueName)
        {
            // create a Service Bus client 
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage("State of Ohio");

                message.MessageId = "9348750lkgjlk";
                message.ApplicationProperties.Add("state","Ohio");
                TimeSpan t = new TimeSpan(0,3,0);
                message.TimeToLive = t;

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }

        
    }
}
