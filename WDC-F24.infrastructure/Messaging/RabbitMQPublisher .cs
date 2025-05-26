using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using WDC_F24.Application.Interfaces;

namespace WDC_F24.Infrastructure.Messaging
{
    public class RabbitMQPublisher : IRabbitMQPublisher
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQPublisher(IConfiguration configuration)
        {
            _factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:Host"] ?? "localhost"
            };
        }

        public void Publish(string queueName, object message)
        {
            using var connection = _factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false);

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
        }
    }
}
