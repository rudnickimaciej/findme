using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EmailService.RabbitMQ
{
    public interface IRabbitMQConsumer : IDisposable
    {
        void ConsumeMessages(string queue, Func<string, Task> handleMessage);
    }
}