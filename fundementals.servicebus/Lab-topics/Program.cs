using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;

namespace Lab_topics
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ManagementClient m_ManagementClient = new ManagementClient("connectionstring");
            string topicPath = "demotopic";
            string subscriptionName = "orderSubscription";

            // create the filter
            var filter = new CorrelationFilter();
            filter.Properties["State"] = "USA";
            var rule = new RuleDescription("Demo", filter);

            var subscriptionDescription = new SubscriptionDescription(topicPath, subscriptionName);

            await m_ManagementClient.CreateSubscriptionAsync(subscriptionDescription, rule);

            Console.WriteLine("Hello World!");
        }
    }
}
