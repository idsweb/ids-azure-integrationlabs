# Azure Service Bus
This folder contains notes on demos on Azure Service Bus.

## Resources
Check out Microsoft Learn's tutorials [here](https://docs.microsoft.com/en-us/learn/modules/implement-message-workflows-with-service-bus/1-introduction).

Note: this folder looks at queues and topics and not relays.

## Note on queues
The resources above recommend that:
1. If your communication an event use Event Hubs or Event Grid*
1. If the message should go to more that one destination use a topic, otherwise use a queue.

__* Event Grid is designed for events, which notify recipients only of an event and do not contain the raw data associated with that event. Azure Event Hubs is designed for high-flow analytics types of events. Azure Service Bus and storage queues are for messages, which can be used for binding the core pieces of any application workflow.__

## Writing code

### Sending messages to a queue
The Microsoft.Azure.ServiceBus nuget package contains classes such as the QueCLient. When sending a message to a queue, for example, use the QueueClient.SendAsync() method with the await keyword.

```c sharp
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;

queueClient = new QueueClient(TextAppConnectionString, "PrivateMessageQueue");
string message = "Sure would like a large pepperoni!";
var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
// use the sendasync method.
await queueClient.SendAsync(encodedMessage);

### Receiving messages
When receiving messages:
1. Create a queue client, note the default is ReceiveMode.PeekLock mode.
```C Sharp
queueClient = new QueueClient(ServiceBusConnectionString, QueueName);
```
Create an instance of MessageHandlerOptions passing it an exception handler
```C sharp
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };
```
The exception handler returns a TaskCompleted. You can get details like the queue from the context.
``` sharp
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            ...
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            ....
            return Task.CompletedTask;
        }
```
Create a method to process the Message that takes a Message and a CancellationToken. Complete the message so that it is not received again. This can be done only if the queue Client is created in ReceiveMode.PeekLock mode (which is the default).
``` C sharp
    static async Task ProcessMessagesAsync(Message message, CancellationToken token)
    {
        // Process the message.
        Console.WriteLine($"Received message: SequenceNumber:{message.SystemProperties.SequenceNumber} Body:{Encoding.UTF8.GetString(message.Body)}");

        
        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
    }
```
Register that with the QueueClient passing in the message processing method and the handler options.
```C sharp
    queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
```
Finally close the queue.

## Service bus topics
To allow multiple components to receive the same message, you use an Azure Service Bus topic. You use the TopicClient class instead of the QueueClient class to send messages and the SubscriptionClient class to receive messages.

If you want to control that specific messages sent to the topic are delivered to particular subscriptions, you can place filters on each subscription in the topic. 
Filters can be one of three types:
1. Boolean Filters. This effectively blocks or switches off the subscription.
1. SQL Filters. 
1. Correlation Filters. If the property in the filter and the property on the message have the same value, it is considered a match.
SQL filters are the most flexible, but they're also the most computationally expensive and slow.

### Sending a single message
To send a message create a TopicClient then send the message with SendAsync.
``` C sharp
topicClient = new TopicClient(TextAppConnectionString, "GroupMessageTopic");
string message = "Are you receiving me Euston over";
var encodedMessage = new Message(Encoding.UTF8.GetBytes(message));
await topicClient.SendAsync(encodedMessage);
```

### Receiving a message from a subscription
Create a SubscriptionClient passing it the name of the connection string and the name of the topic. Register the MessageHandler and the options with the client. Call the SubscriptionClientj.CompleteAsync() method to remove the message from the queue.
``` C sharp
subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, "GroupMessageTopic", "NASA");
subscriptionClient.RegisterMessageHandler(MessageHandler, messageHandlerOptions);
await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);






