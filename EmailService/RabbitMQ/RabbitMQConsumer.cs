using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EmailService.RabbitMQ
{
    public class RabbitMQConsumer : IDisposable, IRabbitMQConsumer
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQConsumer(string uri)
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uri);
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }
        public void ConsumeMessages(string queue, Func<string, Task> handleMessage)
        {
            _channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                await handleMessage(message);
            };

            _channel.BasicConsume(queue: queue, autoAck: true, consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}