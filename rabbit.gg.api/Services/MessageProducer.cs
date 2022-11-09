using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace rabbit.gg.api.Services;

public class MessageProducer : IMessageProducer
{
    public void PublishMessage<T>(T message)
    {
        var connection = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "user",
            Password = "password",
            VirtualHost = "/"
        }.CreateConnection();

        using var channel = connection.CreateModel();
        
        channel.QueueDeclare(
            queue: "orders",
            durable: true,
            exclusive: true);

        var body = Encoding.UTF8.GetBytes(
            JsonSerializer.Serialize(message));
        
        channel.BasicPublish(
            exchange: "",
            routingKey: "orders",
            body: body);
    }
}
